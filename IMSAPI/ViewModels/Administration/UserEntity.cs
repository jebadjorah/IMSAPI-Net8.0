using IMSAPI.Models.Administration;

namespace IMSAPI.ViewModels.Administration
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        //public RoleModel Role { get; set; }
        public int CompanyId { get; set; }
        //public CompanyModel Company { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
    }
}
