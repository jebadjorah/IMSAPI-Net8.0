using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;

namespace IMSAPI.Services.Administration
{
    public class EntityService : IEntityService
    {
        private readonly AppDbContext _config;
        public EntityService(AppDbContext config)
        {
            _config = config;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.entityModels.FindAsync(id);
                if (item != null)
                {
                    _config.entityModels.Remove(item);
                    await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            }

        public async Task<IEnumerable<Entity>> Get(int companyId, int id = 0)
        {
        var objList = new List<Entity>();
        try
        {
            
                     var obj = await _config.entityModels
                    .Where(x => x.CompanyId ==companyId && (id ==0 || x.Id ==id)) .ToListAsync();
                //  var companyModels = await _config.entityModels.Where(x => x.CompanyId ==2).ToListAsync();
                if (obj != null && obj.Count > 0)
                {
                    objList = obj
                        .Select(x => new Entity
                        {
                            EntityCode = x.EntityCode,
                            EntityName = x.EntityName,
                            ParentId = x.ParentId,
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

        public async Task<bool> SaveUpdate(Entity obj)
        {
        try
        {
                if (obj.Id > 0)
                {
                    //var result = await _config.entityModels.FirstOrDefaultAsync(x => x.Id == obj.Id);
                    var result = await _config.entityModels.FindAsync(obj.Id);
                    if (result != null)
                    {
                        result.EntityCode = obj.EntityCode;
                        result.EntityName = obj.EntityName;
                        result.IsActive = obj.IsActive;
                        result.ParentId = obj.ParentId;
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

                    var inputObj = new EntityModel
                    {
                    EntityCode = obj.EntityCode,
                    EntityName = obj.EntityName,
                    IsActive = obj.IsActive,
                    ParentId = obj.ParentId,
                    CompanyId = obj.CompanyId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = obj.CreatedBy
                    };
                    await _config.entityModels.AddAsync(inputObj);
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
