using IMSAPI.DB;
using IMSAPI.Filters;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Immutable;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IMSAPI.Controllers.Administration
{
    [Authorize]
    [CustomAuthorizationFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly int companyId;
        private readonly int userId;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor) {
            _userService = userService;
            companyId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("CompanyId").Value);
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
        }
       
        [HttpGet]
        public async Task<IEnumerable<UserEntity>> Get(int roleId = 0, int id = 0)
        {
            try
            {
                return await _userService.Get(companyId, roleId, id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<UserEntity> GetbyId(int id)
        {
            try
            {
                var objlist = (List<UserEntity>)await _userService.Get(companyId, 0, id);
                if (objlist.Count > 0)
                {
                    return objlist[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveUpdate(UserEntity obj)
        {
            try
            {
                obj.CompanyId = companyId;
                if (await _userService.SaveUpdate(obj))
                {
                    return Ok();
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _userService.Delete(id))
                {
                    return Ok();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
