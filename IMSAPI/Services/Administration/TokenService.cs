using IMSAPI.ViewModels.Administration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IMSAPI.Services.Administration
{
    public class TokenService
    {
        private const int ExpirationMinutes = 43200;
        private readonly ILogger<TokenService> _logger;

        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }

        public string GenerateJWTToken(ClaimResponse user)
        {
            var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];
            var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier ,user.UserName),
                new Claim(ClaimTypes.Role ,user.RoleName.ToString()),
                new Claim("CompanyId",user.CompanyId.ToString()),
                new Claim("UserId",user.UserId.ToString()),
                new Claim("RoleId",user.RoleId.ToString())
            };

            var jwtToken = new JwtSecurityToken(
                 new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(ExpirationMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(symmetricSecurityKey)
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public static string GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
        {
            // Implement logic to generate a new access token from the refresh token
            // Verify the refresh token and extract necessary information (e.g., user ID)
            // Then generate a new access token

            // For demonstration purposes, return a new token with an extended expiry
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(ExpirationMinutes), // Extend expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
