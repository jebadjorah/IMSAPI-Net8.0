using IMSAPI.ViewModels.Administration;

namespace IMSAPI.Services.Administration.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> Get(int companayId, int roleId = 0, int id = 0);
        Task<bool> SaveUpdate(UserEntity obj);
        Task<bool> Delete(int id);
        Task<ClaimResponse> LoginUser(string userName, string password);
    }
}
