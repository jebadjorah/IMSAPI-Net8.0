using IMSAPI.ViewModels.Administration;

namespace IMSAPI.Services.Administration.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleEntity>> Get(int companyId, int id = 0);
        Task<bool> SaveUpdate(RoleEntity obj);
        Task<bool> Delete(int id);
    }
}
