using IMSAPI.ViewModels.Administration;

namespace IMSAPI.Services.Administration.Interface
{
    public interface IRolePrivilageService
    {
        Task<IEnumerable<RolePrivilageEntity>> Get(int companayId, int roleId = 0, int id = 0);
       //  Task<bool> GetAccess(int companayId, int roleId, int userId, string actionName, string controllerName);
        bool GetAccess(int companayId, int roleId, int userId, string actionName, string controllerName);
        Task<bool> SaveUpdate(RolePrivilageEntity obj);
        Task<bool> Delete(int id);
        Task GetAllControllerANDActionName();
    }

}
