using IMSAPI.ViewModels.Administration;

namespace IMSAPI.Services.Administration.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> Get(int companayId, int roleId = 0, int id = 0);
        Task<bool> SaveUpdate(int companayId,UserEntity obj);
        Task<bool> Delete(int id);
    }
}
