using IMSAPI.Services.Administration.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace IMSAPI.Services.Administration
{
    public class AzureTokenValidatorService : IAzureTokenValidatorService
    {
        private readonly string _issuer;
        private readonly string _audience;
        //private readonly SecurityKey _signingKey;

        public AzureTokenValidatorService()
        {
            _issuer = "https://login.microsoftonline.com/70a2e666-c131-41ef-9b49-87f8c078b3cd/v2.0";
            _audience = "9ab165f4-76d3-4c48-8164-aaffe9434525";
        }

        public async Task<bool> ValidateAzureToken(string jwtToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(jwtToken);

                if (token.Issuer != _issuer)
                {
                    throw new Exception("Invalid issuer");
                }

                //    string username = token?.Claims?.FirstOrDefault(c => c.Type == "preferred_username")?.Value;

                // Retrieve the JWKS (JSON Web Key Set) from Microsoft's endpoint
                var jwksUrl = "https://login.microsoftonline.com/common/discovery/keys";
                var httpClient = new HttpClient();
                var jwksResponse = httpClient.GetStringAsync(jwksUrl).Result;
                var jwks = JObject.Parse(jwksResponse);

                // Find the appropriate key based on the key ID (kid) from the JWT header
                var kid = token.Header.Kid;
                var keys = jwks["keys"];
                JObject key = null;
                foreach (var k in keys)
                {
                    if (k["kid"].ToString() == kid)
                    {
                        key = (JObject)k;
                        break;
                    }
                }

                if (key == null)
                {
                    throw new Exception("Key not found");
                }

                // Construct a security key from the key parameters
                var keyParams = new RSAParameters
                {
                    Exponent = Base64UrlDecode(key["e"].ToString()),
                    Modulus = Base64UrlDecode(key["n"].ToString())
                };
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(keyParams);
                var securityKey = new RsaSecurityKey(rsa);

                // Create validation parameters
                var validationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = true,
                    ValidateIssuer = true,
                    ValidateAudience = true, // Adjust this as needed
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,//"https://login.microsoftonline.com/70a2e666-c131-41ef-9b49-87f8c078b3cd/v2.0",
                    ValidAudience = _audience,//"9ab165f4-76d3-4c48-8164-aaffe9434525",
                    IssuerSigningKey = securityKey,
                };

                try
                {
                    // Try to validate the token
                    var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out _);
                    return true;
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> GetUserInfoAsync(string jwtToken)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set up the request
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    //client.BaseAddress = new Uri("https://graph.microsoft.com/v2.0/");
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //// Make the request to /me endpoint
                    //HttpResponseMessage response = await client.GetAsync("me");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                    client.BaseAddress = new Uri("https://graph.microsoft.com/v1.0/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Add debug headers
                    client.DefaultRequestHeaders.Add("client-request-id", Guid.NewGuid().ToString());
                    client.DefaultRequestHeaders.Add("return-client-request-id", "true");
                    client.DefaultRequestHeaders.Add("SdkVersion", "Graph-dotnet-3.0");

                    HttpResponseMessage response = await client.GetAsync("me");


                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve user info. Status code: {response.StatusCode}");
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private byte[] Base64UrlDecode(string input)
        {
            string padded = input.Length % 4 == 0 ? input : input + "====".Substring(input.Length % 4);
            string base64 = padded.Replace("_", "/").Replace("-", "+");
            return Convert.FromBase64String(base64);
        }
    }
}
