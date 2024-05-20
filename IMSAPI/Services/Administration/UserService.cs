using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IMSAPI.Services.Administration
{
    public class UserService :IUserService
    {
        private readonly AppDbContext _config;
        public UserService(AppDbContext config)
        {
            _config = config;
        }

        public async Task<ClaimResponse> LoginUser(string userName, string Password)
        {
            ClaimResponse response = null;
            try {
                var item = await _config.userModels.Where(x => x.UserId == userName && x.Password == Password).FirstOrDefaultAsync();
                var roleItem = await _config.roleModels.Where(x => x.Id == item.RoleId).FirstOrDefaultAsync();
                if (item != null  && roleItem !=null)
                {
                    response = new ClaimResponse();
                    response.UserName = item.UserId;
                    response.UserEmail = item.Email;
                    response.RoleName = roleItem.RoleName.ToString();
                    response.CompanyId = item.CompanyId;
                    response.RoleId = item.RoleId;
                    response.UserId = item.Id;
                    return response;
                }
                
            }
            catch(Exception ex)
            {
                return null;
            }
            return response;
        }
        public async Task<ClaimResponse> LoginAD(string userEmail)
        {
            ClaimResponse response = null;
            try
            {
                var item = await _config.userModels.Where(x => x.Email == userEmail  || x.UserId == userEmail).FirstOrDefaultAsync();
                var roleItem = await _config.roleModels.Where(x => x.Id == item.RoleId).FirstOrDefaultAsync();
                if (item != null && roleItem != null)
                {
                    response = new ClaimResponse();
                    response.UserName = item.UserId;
                    response.UserEmail = item.Email;
                    response.RoleName = roleItem.RoleName.ToString();
                    response.CompanyId = item.CompanyId;
                    response.RoleId = item.RoleId;
                    response.UserId = item.Id;
                    return response;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return response;
        }
        public async Task<bool> UpdateRefereshToken(int empoyeeId, string refershToken)
        {
            //try
            //{
            //    var result = await _config.RefreshTokenModel.FirstOrDefaultAsync(x => x.EmployeeId == empoyeeId);
            //    if (result != null)
            //    {
            //        result.RefreshTokenExpiration = DateTime.Now.AddDays(RefereshTokenBuffer);
            //        result.RefreshToken = refershToken;
            //        await _config.SaveChangesAsync();
            //    }
            //    else
            //    {
            //        var inputObj = new RefreshTokenModel
            //        {
            //            EmployeeId = empoyeeId,
            //            RefreshTokenExpiration = DateTime.Now.AddDays(RefereshTokenBuffer),
            //            RefreshToken = refershToken
            //        };
            //        await _config.RefreshTokenModel.AddAsync(inputObj);
            //        await _config.SaveChangesAsync();
            //    }
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    _logger.CreateErrorLog(ex, "UpdateRefereshToken");
            //    return false;
            //}
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.userModels.FindAsync(id);
                if (item != null)
                {
                    item.IsDeleted = true;  // Soft delete
                    await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserEntity>> Get(int companyId, int roleId=0, int id = 0)
        {
            var objList = new List<UserEntity>();
            try
            {
                var obj = await _config.userModels.Where(x => x.CompanyId== companyId && x.IsDeleted==false && (x.Id == id || id == 0)  && (x.RoleId == roleId || roleId == 0)).ToListAsync();
                if (obj != null)
                {
                    objList = obj
                        .Select(x => new UserEntity
                        {
                            UserId = x.UserId,
                            Password = x.Password,
                            Email = x.Email,
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

        public async Task<bool> SaveUpdate(UserEntity obj)
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
                        result.Email = obj.Email;
                        result.IsActive = obj.IsActive;
                        result.RoleId = obj.RoleId;
                        result.UpdatedOn = DateTime.Now;
                        result.UpdatedBy = obj.CreatedBy;
                        result.CompanyId = obj.CompanyId;
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
                        Email = obj.Email,
                        IsActive = obj.IsActive,
                        RoleId = obj.RoleId,
                        CompanyId = obj.CompanyId,
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
