using IMSAPI.Models.Administration;

namespace IMSAPI.ViewModels.Administration
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }
    }
}
