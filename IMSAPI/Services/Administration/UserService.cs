using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;

namespace IMSAPI.Services.Administration
{
    public class UserService :IUserService
    {
        private readonly AppDbContext _config;
        public UserService(AppDbContext config)
        {
            _config = config;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.userModels.FindAsync(id);
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

        public async Task<IEnumerable<UserEntity>> Get(int companayId, int roleId=0, int id = 0)
        {
            var objList = new List<UserEntity>();
            try
            {
                var obj = await _config.userModels.Where(x => x.IsDeleted==false && (x.Id == id || id == 0)  && (x.RoleId == roleId || roleId == 0)).ToListAsync();
                if (obj != null)
                {
                    objList = obj
                        .Select(x => new UserEntity
                        {
                            UserId = x.UserId,
                            Password = x.Password,
                            RoleId = x.RoleId,
                            IsActive = x.IsActive,
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

        public async Task<bool> SaveUpdate(int companayId,UserEntity obj)
        {
            try
            {
                if (obj.Id > 0)
                {
                    var result = await _config.userModels.FindAsync(obj.Id);
                    if (result != null)
                    {
                        result.UserId = obj.UserId;
                        result.Password = obj.Password;
                        result.IsActive = obj.IsActive;
                        result.RoleId = obj.RoleId;
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

                    var inputObj = new UserModel
                    {
                        UserId = obj.UserId,
                        Password = obj.Password,
                        IsActive = obj.IsActive,
                        RoleId = obj.RoleId,
                      //  CompanyId = companayId,
                        CreatedOn = DateTime.Now,
                        CreatedBy = obj.CreatedBy
                    };
                    await _config.userModels.AddAsync(inputObj);
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
