using Azure.Core;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;


namespace IMSAPI.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        public LoginController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(AuthRequest  obj)
        {
            using (var adContext = new PrincipalContext(ContextType.Domain, "smartv.ae"))
            {
                var result = adContext.ValidateCredentials(obj.UserName, obj.Password);
                if (result)
                {
                    Console.WriteLine("AD Authentication successfully login");
                }
            }
     

    var claimResponse = await _userService.LoginUser(obj.UserName, obj.Password);

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
    }
}

