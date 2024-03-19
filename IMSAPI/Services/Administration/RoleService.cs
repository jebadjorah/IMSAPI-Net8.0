using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;

namespace IMSAPI.Services.Administration
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _config;
        public RoleService(AppDbContext config)
        {
            _config = config;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.roleModels.FindAsync(id);
                if (item != null)
                {
                    item.IsDeleted = true; // soft delete
                    await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<RoleEntity>> Get(int companyId, int id = 0)
        {
            var objList = new List<RoleEntity>();
            try
            {
                    var obj = await _config.roleModels
                    .Where(x => x.CompanyId== companyId && x.IsDeleted== false && (x.Id== id || id ==0)).ToListAsync();
                if (obj != null && obj.Count >0)
                {
                    objList = obj
                       .Select(x => new RoleEntity
                       {
                           RoleName = x.RoleName,
                           RoleDescription = x.RoleDescription,
                           Sequence = x.Sequence,
                           IsActive = x.IsActive,
                           CompanyId = x.CompanyId,
                           Id = x.Id
                       }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return objList;
        }

        public async Task<bool> SaveUpdate(RoleEntity obj)
        {
            try
            {
                if (obj.Id > 0)
                {
                    var result = await _config.roleModels.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == obj.Id);
                    if (result != null)
                    {
                        result.RoleName = obj.RoleName;
                        result.RoleDescription = obj.RoleDescription;
                        result.IsActive = obj.IsActive;
                        result.Sequence = obj.Sequence;
                        result.CompanyId = obj.CompanyId;
                        result.UpdatedOn = DateTime.Now;
                        result.UpdatedBy = obj.CreatedBy;
                        await _config.SaveChangesAsync();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var inputObj = new RoleModel
                    {
                        RoleName = obj.RoleName,
                        RoleDescription = obj.RoleDescription,
                        IsActive = obj.IsActive,
                        Sequence = obj.Sequence,
                        CompanyId = obj.CompanyId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = obj.CreatedBy
                    };
                    await _config.roleModels.AddAsync(inputObj);
                    await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
