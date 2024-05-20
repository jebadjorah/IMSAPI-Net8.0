using Azure.Core;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;


namespace IMSAPI.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IAzureTokenValidatorService _azureTokenValidatorService;
        public LoginController(IUserService userService, TokenService tokenService,IConfiguration configuration, IAzureTokenValidatorService azureTokenValidatorService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration= configuration;
            _azureTokenValidatorService = azureTokenValidatorService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(AuthRequest obj)
        {
            var authentiationType = 1;
            try
            {
                authentiationType = Convert.ToInt32(_configuration.GetSection("AuthentiationType").Value);
            }
            catch(Exception ex){
            }

            var claimResponse =new ClaimResponse();
            bool isADAuthenticated = false;
            if (authentiationType == (int) AuthenticationType.ADAuth)
            {
                // Domain AD Authentication 
                isADAuthenticated = await CheckADAuth(obj);
                if (isADAuthenticated)
                {
                    claimResponse = await _userService.LoginAD(obj.UserName);
                }
            }
            else if(authentiationType == (int) AuthenticationType.AzureADAuth)
            {
                // writh Azure AD auth configurations....
            }
            else if(authentiationType == (int) AuthenticationType.BasicAuth || !isADAuthenticated)
            {
                    // Basic Authentication - DB Validation
                claimResponse = await _userService.LoginUser(obj.UserName, obj.Password);
            }

            if (claimResponse != null)
            {
                var accessToken = _tokenService.GenerateJWTToken(claimResponse);

                return Ok(new AuthResponse
                {
                    UserName = obj.UserName,
                    //Email = obj.Email,
                    Token = accessToken,
                    //expires= ac
                });
            }
            else
            {
                return BadRequest("Incorrect UserName / Password");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string jwtToken)
        {
            return null;
        }
        [HttpPost]
        public async Task<IActionResult> RefershToken(string jwtToken)
        {
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAzureAD(AzureLoginRequest obj)
        {
            // obj.AzureToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IkwxS2ZLRklfam5YYndXYzIyeFp4dzFzVUhIMCJ9.eyJhdWQiOiI5YWIxNjVmNC03NmQzLTRjNDgtODE2NC1hYWZmZTk0MzQ1MjUiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vNzBhMmU2NjYtYzEzMS00MWVmLTliNDktODdmOGMwNzhiM2NkL3YyLjAiLCJpYXQiOjE3MTQ5NzA4OTYsIm5iZiI6MTcxNDk3MDg5NiwiZXhwIjoxNzE0OTc0Nzk2LCJhaW8iOiJBVFFBeS84V0FBQUE4WFhFdG5WYzZZZ2ZhUmIzMnFERWF0K1dGaFUwcm5qR0RMNExwQUNqSXBLREZTblg0cENxYWoyRHZKVjAwbmtYIiwibmFtZSI6IkhhemVtIE5haW0gW1NWIHwgQVVIIHwgUHJvZHVjdCBEZXB0Ll0iLCJub25jZSI6IjAxOGY0YzNmLWI3ZjgtNzMyOC05MjVmLWE3OTc4OWM3ZWIyNCIsIm9pZCI6IjY2ZjM4ZDgxLWIxNTQtNGExZi05NGYwLTg0ZmY0MGRjMGM4NyIsInByZWZlcnJlZF91c2VybmFtZSI6IkhhemVtLm5Ac21hcnR2LmFlIiwicmgiOiIwLkFVY0FadWFpY0RIQjcwR2JTWWY0d0hpenpmUmxzWnJUZGtoTWdXU3FfLWxEUlNVTkFUby4iLCJzdWIiOiI3Y2VMZkpzdTJnLUVCUUNZMXhNd1ZGdXpiYkxvd1JsV1I4YXZDYWkwdE93IiwidGlkIjoiNzBhMmU2NjYtYzEzMS00MWVmLTliNDktODdmOGMwNzhiM2NkIiwidXRpIjoiMDM1YkU4M3BEMEczTVl4Y3hVd3VBQSIsInZlciI6IjIuMCJ9.bAB9QVlh70VrbDQXZD31ASrReNyuwMiUVYENNJvXnzCsJb5TQThnScZCMkSlG0kaZkLK7V678-wfpQymI7qjdJokOjlsYOg2b3xo6HtPK4g9m7tz8NrReJtFc5aFfBtzkdb3d38oz27T03mUkNNad1Gg4ACe32OwsqIUDWhF39mC1fQd3ZbkLlCTc3CkAp0pOQJS8RYnqdUjc7tq12IZ9CSNKGawRIYd23Ldv38375wAeljSmJmOPGrc6SkHfpvI1HugQ53EM5T26dAyALr5RotCTluStOnYAYWW0cLHOj5Q94l5SvYnTRjAGU-j30-PGH6fpIiNNK9GixjrljELtg";
            //obj.AzureToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IkwxS2ZLRklfam5YYndXYzIyeFp4dzFzVUhIMCJ9.eyJhdWQiOiI5YWIxNjVmNC03NmQzLTRjNDgtODE2NC1hYWZmZTk0MzQ1MjUiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vNzBhMmU2NjYtYzEzMS00MWVmLTliNDktODdmOGMwNzhiM2NkL3YyLjAiLCJpYXQiOjE3MTQ5ODAwODksIm5iZiI6MTcxNDk4MDA4OSwiZXhwIjoxNzE0OTgzOTg5LCJhaW8iOiJBVFFBeS84V0FBQUFlTXAzczBVU0wvSnJEd2xXWnIva2tSWlVuSUkvOXNFL2ZIUFMrUnZUVnlZclV1dGpnWUhTbjRySnFUVmN6c3phIiwibmFtZSI6IkhhemVtIE5haW0gW1NWIHwgQVVIIHwgUHJvZHVjdCBEZXB0Ll0iLCJub25jZSI6IjAxOGY0Y2NiLWQ4NjQtN2M1MC04ZTllLTE0NzVhNWZlZjhkOSIsIm9pZCI6IjY2ZjM4ZDgxLWIxNTQtNGExZi05NGYwLTg0ZmY0MGRjMGM4NyIsInByZWZlcnJlZF91c2VybmFtZSI6IkhhemVtLm5Ac21hcnR2LmFlIiwicmgiOiIwLkFVY0FadWFpY0RIQjcwR2JTWWY0d0hpenpmUmxzWnJUZGtoTWdXU3FfLWxEUlNVTkFUby4iLCJzdWIiOiI3Y2VMZkpzdTJnLUVCUUNZMXhNd1ZGdXpiYkxvd1JsV1I4YXZDYWkwdE93IiwidGlkIjoiNzBhMmU2NjYtYzEzMS00MWVmLTliNDktODdmOGMwNzhiM2NkIiwidXRpIjoiZEtwRHl4RWhXRWlsMGJocExLUXhBQSIsInZlciI6IjIuMCJ9.VOs78OCUkfx9jaYQVa08HUmnGCJSKgEM6p3SZdr52QlYwrsTTt77iYA344_LOgE5sZTsTjDxMg1bJ-Y3g2O6BzkQVwUHPY4wgBRqTZiPmVTaxKNTwxfU2SuZ4AnikFRRmnjatnp8oONn6wYTuLzCmh9FVDSNgQuKKqBhamXaex1W87htxt0j2W2MuH80mp8TIO0zW3Jh_y9B6G17i3CoUP0Bs24s730CDuwCrOUA3v7JZuEWbsbqrtzVLtRqAW0Fa3jbAghZvmP1Xrk_u2YlelwA8p5Z5JZ6dhzShl11gfpT5sX_OWft6NDhLHPDpx4GqOUf8lvhDf8pIznWEAou0g";

            var azureValidate = await _azureTokenValidatorService.ValidateAzureToken(obj.AzureToken);
            await _azureTokenValidatorService.GetUserInfoAsync(obj.AzureToken);

            if (azureValidate)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(obj.AzureToken);

               

                string userName = obj.UserName;
                if (token != null)
                {
                    try
                    {
                        userName = token?.Claims?.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                    }
                    catch (Exception ex)
                    {
                        userName = obj.UserName;
                    }
                }
                var claimResponse = await _userService.LoginAD(userName);
                if (claimResponse != null && claimResponse.UserId != null)
                {
                    var accessToken = _tokenService.GenerateJWTToken(claimResponse);
                    var refreshToken = GenerateRefreshToken();
                    await _userService.UpdateRefereshToken(claimResponse.UserId, refreshToken.ToString());
                    // db save 

                    return Ok(new AuthResponse
                    {
                        UserName = obj.UserName,
                        //Email = obj.Email,
                        Token = accessToken,
                        //expires= ac
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("Invalid Azure Token");
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private async Task<Boolean> CheckADAuth(AuthRequest obj)
        {
            bool isADAuthenticated = false;
            try
            {
                var domain = "";
                try
                {
                    domain = _configuration.GetSection("Domain").Value;
                }
                catch (Exception ex)
                {

                }
                using (var adContext = new PrincipalContext(ContextType.Domain, domain))
                {
                    isADAuthenticated = adContext.ValidateCredentials(obj.UserName, obj.Password);
                    if (isADAuthenticated)
                    {
                        Console.WriteLine("AD Authentication successfully login");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return isADAuthenticated;
        }
    }
}

