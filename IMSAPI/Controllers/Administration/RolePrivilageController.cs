using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public RolePrivilageController(IRolePrivilageService rolePrivilageService) 
        {
            _rolePrivilageService = rolePrivilageService;
            companyId = 2;
        }
        [HttpGet]
        public async Task<IEnumerable<RolePrivilageEntity>> Get()
        {
            try
            {

                await _rolePrivilageService.GetAllControllerANDActionName();
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
    }
}
