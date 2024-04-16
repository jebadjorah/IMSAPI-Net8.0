using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace IMSAPI.Controllers.Administration
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolePrivilageController : ControllerBase
    {
        private readonly IRolePrivilageService _rolePrivilageService;
        private readonly int companyId;
        private readonly int userId;
        public RolePrivilageController(IRolePrivilageService rolePrivilageService, IHttpContextAccessor httpContextAccessor) 
        {
            _rolePrivilageService = rolePrivilageService;
            companyId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("CompanyId").Value);
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
        }
        [HttpGet]
        public async Task<IEnumerable<RolePrivilageEntity>> Get()
        {
            try
            {
                return await _rolePrivilageService.Get(companyId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<RolePrivilageEntity> GetbyId(int id)
        {
            try
            {
                List<RolePrivilageEntity> objlist = (List<RolePrivilageEntity>)await _rolePrivilageService.Get(companyId, 0,id);
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
        public async Task<ActionResult> SaveUpdate(RolePrivilageEntity obj)
        {
            try
            {
                if (await _rolePrivilageService.SaveUpdate(obj))
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
                if (await _rolePrivilageService.Delete(id))
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

        [HttpGet]
        public async Task<IEnumerable<ControllersDto>> GetAllControllerAndActions()
        {
            try
            {   
                return await _rolePrivilageService.GetAllControllerANDActionName();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
