using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Mvc;

namespace IMSAPI.Services.Administration.Interface
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyEntity>> Get(int id = 0);
        Task<bool> SaveUpdate(CompanyEntity obj);
        Task<bool> Delete(int id);
    }
}
