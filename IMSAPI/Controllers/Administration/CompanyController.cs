using IMSAPI.DB;
using IMSAPI.Filters;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Reflection.Metadata;
using System.Security.Claims;
//using System.Security.Claims;

namespace IMSAPI.Controllers.Administration
{
    [Authorize]
    [CustomAuthorizationFilter]
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class CompanyController : ControllerBase
    {
        
        private readonly ICompanyService _companyService;
        private readonly int companyId;
        private readonly int userId;
        public CompanyController(ICompanyService companyService, IHttpContextAccessor httpContextAccessor) 
        {
            _companyService = companyService;
            companyId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("CompanyId").Value);
            userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("UserId").Value);

        }
        [HttpGet]
        public async Task<IEnumerable<CompanyEntity>> Get()
        {
            try
            {
                return await _companyService.Get();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<CompanyEntity> GetbyId(int id)
        {
            try
            {
                List<CompanyEntity> objlist = (List<CompanyEntity>) await _companyService.Get(id);
                if(objlist.Count>0)
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
        public async Task<ActionResult> SaveUpdate(CompanyEntity obj)
        {
            try
            {
               if(await _companyService.SaveUpdate(obj))
                {
                    return Ok();
                }
                else
                {
                    return NoContent();
                }
                
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _companyService.Delete(id))
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
