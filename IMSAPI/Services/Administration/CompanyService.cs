using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace IMSAPI.Services.Administration
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _config;
       // private readonly ILogger _logger;
        public CompanyService(AppDbContext config)
        {
            _config = config;
           // _logger = logger;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.companyModels.FindAsync(id);
                if(item != null)
                {
                    _config.companyModels.Remove(item);
                    await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<CompanyEntity>> Get(int id = 0)
        {
            var objList = new List<CompanyEntity>();
            try
            {
                if (id > 0)
                {
                    CompanyModel obj = await _config.companyModels.FindAsync(id);
                    if (obj == null)
                    {
                        return null;
                    }
                        var item = new CompanyEntity();
                        item.CompanyCode = obj.CompanyCode;
                        item.CompanyName = obj.CompanyName;
                        item.CompanyAddress = obj.CompanyAddress;
                        item.IsActive = obj.IsActive;
                        item.Email = obj.Email;
                        item.ContactNumber = obj.ContactNumber;
                        item.Id = obj.Id;
                        objList.Add(item);
                    
                }
                else
                {
                    List<CompanyModel>companyModels= await _config.companyModels.ToListAsync();
                    if(companyModels == null)
                    {
                        return null;
                    }
                    foreach (var obj in companyModels)
                    {
                        var item = new CompanyEntity();
                        item.CompanyCode = obj.CompanyCode;
                        item.CompanyName = obj.CompanyName;
                        item.CompanyAddress = obj.CompanyAddress;
                        item.IsActive = obj.IsActive;
                        item.Email = obj.Email;
                        item.ContactNumber = obj.ContactNumber;
                        item.Id = obj.Id;
                        objList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return objList;
        }
        public async Task<bool> SaveUpdate(CompanyEntity obj)
        {
            try
            {
                    if (obj.Id > 0)
                    {
                        var result = await _config.companyModels.FirstOrDefaultAsync(x => x.Id == obj.Id);
                        if (result != null)
                        {
                            result.CompanyCode = obj.CompanyCode;
                            result.CompanyName = obj.CompanyName;
                            result.CompanyAddress = obj.CompanyAddress;
                            result.IsActive = obj.IsActive;
                            result.Email = obj.Email;
                            result.ContactNumber = obj.ContactNumber;
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
                        var result = new CompanyModel();
                        result.CompanyCode = obj.CompanyCode;
                        result.CompanyName = obj.CompanyName;
                        result.CompanyAddress = obj.CompanyAddress;
                        result.IsActive = obj.IsActive;
                        result.Email = obj.Email;
                        result.ContactNumber = obj.ContactNumber;
                        result.CreatedOn = DateTime.Now;
                        result.CreatedBy = obj.CreatedBy;
                        await _config.companyModels.AddAsync(result);
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
