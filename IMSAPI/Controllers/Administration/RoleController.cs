using IMSAPI.Filters;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace IMSAPI.Controllers.Administration
{
    [Authorize]
    [CustomAuthorizationFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly int companyId;
        private readonly int userId;
        public RoleController(IRoleService roleService, IHttpContextAccessor httpContextAccessor) {
            _roleService = roleService;
            companyId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("CompanyId").Value);
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
        }
        [HttpGet]
        public async Task<IEnumerable<RoleEntity>> Get()
        {
            try
            {
                return await _roleService.Get(companyId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<RoleEntity> GetbyId(int id)
        {
            try
            {
                List<RoleEntity> objlist = (List<RoleEntity>)await _roleService.Get(companyId, id);
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
        public async Task<ActionResult> SaveUpdate(RoleEntity obj)
        {
            try
            {
                obj.CompanyId = companyId;
                if (await _roleService.SaveUpdate(obj))
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
                if (await _roleService.Delete(id))
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
