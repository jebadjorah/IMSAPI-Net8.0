using IMSAPI.Filters;
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
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _entityService;
        private readonly int companyId;
        private readonly int userId;
        public EntityController(IEntityService entityService, IHttpContextAccessor httpContextAccessor)
        {
            _entityService = entityService;
            companyId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("CompanyId").Value);
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);
        }
        [HttpGet]
        public async Task<IEnumerable<Entity>> Get()
        {
            try
            {
                return await _entityService.Get(companyId);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<Entity> GetbyId(int id)
        {
            try
            {
                List<Entity> objlist = (List<Entity>)await _entityService.Get(companyId, id);
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
        public async Task<ActionResult> SaveUpdate(Entity obj)
        {
            try
            {
                obj.CompanyId = companyId;
                if (await _entityService.SaveUpdate(obj))
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
                if (await _entityService.Delete(id))
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
