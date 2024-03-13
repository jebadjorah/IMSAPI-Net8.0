using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSAPI.Models.Administration
{
    [Table("Tbl_Roles")]
    public class RoleModel
    {
        [Key]
        public  int Id { get; set; }
        [Required]
        public  string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime?  UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set;}
        public ICollection<UserModel> Users { get; set; }
        public int CompanyId { get; set; }
        public CompanyModel Company { get; set; }
    }
    [Table("Tbl_Users")]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }
        public RoleModel Role { get; set; }
        public int CompanyId { get; set; }
        public CompanyModel Company { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
