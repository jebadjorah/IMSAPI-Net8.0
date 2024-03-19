using IMSAPI.ViewModels.Administration;

namespace IMSAPI.Services.Administration.Interface
{
    public interface IEntityService
    {
        Task<IEnumerable<Entity>> Get(int companyId, int id = 0);
        Task<bool> SaveUpdate(Entity obj);
        Task<bool> Delete(int id);
    }
}
