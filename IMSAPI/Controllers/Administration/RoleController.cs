﻿using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMSAPI.Controllers.Administration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly int _companyId;
        public RoleController(IRoleService roleService) {
            _roleService = roleService;
            _companyId = 2; 
        }
        [HttpGet]
        public async Task<IEnumerable<RoleEntity>> Get()
        {
            try
            {
                return await _roleService.Get(_companyId);
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
                List<RoleEntity> objlist = (List<RoleEntity>)await _roleService.Get(_companyId,id);
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
                obj.CompanyId = _companyId;
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
