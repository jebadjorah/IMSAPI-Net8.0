using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // return null;

            //var accessToken = _tokenService.CreateToken(obj);
            var accessToken = _tokenService.GenerateJWTToken(obj);

            return Ok(new AuthResponse
            {
                UserName = obj.UserName,
                //Email = obj.Email,
                Token = accessToken,
                //expires= ac
            });
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

