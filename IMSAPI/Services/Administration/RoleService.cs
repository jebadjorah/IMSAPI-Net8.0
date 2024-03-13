using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace IMSAPI.Services.Administration
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _config;
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.roleModels.FindAsync(id);
                if (item != null)
                {
                    item.IsDeleted = true;
                    await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<RoleEntity>> Get(int id = 0)
        {
            var objList = new List<RoleEntity>();
            try
            {
                if (id > 0)
                {
                    //RoleModel obj = await _config.roleModels.FindAsync(id);
                    //RoleModel obj = await _config.roleModels.Where(x=> x.Id == id && x.IsDeleted ==false).FirstAsync();
                    RoleModel obj = await _config.roleModels.FirstOrDefaultAsync(x => x.Id==id && x.IsDeleted==true);
                    var item = new RoleEntity
                    {
                        RoleName = obj.RoleName,
                        RoleDescription = obj.RoleDescription,
                        Sequence = obj.Sequence,
                        IsActive = obj.IsActive,
                        CompanyId = obj.CompanyId,
                        Id = obj.Id
                    };
                    objList.Add(item);
                }
                else
                {
                    List<RoleModel> companyModels = await _config.roleModels.ToListAsync();
                    //List<RoleModel> companyModels = await _config.roleModels.Where(x => x.IsDeleted== false).ToListAsync();
                     objList = companyModels
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
                    var result = await _config.roleModels.FirstOrDefaultAsync(x => x.Id == obj.Id);
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
