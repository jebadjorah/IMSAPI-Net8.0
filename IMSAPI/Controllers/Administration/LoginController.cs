using Azure.Core;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;


namespace IMSAPI.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;
        public LoginController(IUserService userService, TokenService tokenService,IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration= configuration;
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

