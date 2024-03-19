using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSAPI.Models.Administration
{
    [Table("Tbl_Company")]
    public class CompanyModel
    {
        [Key]
        public  int Id { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public bool IsActive { get; set;}
        public string Email { get; set;}
        public string ContactNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public ICollection<EntityModel> Entitys { get; set; }
        public ICollection<RoleModel> Roles { get; set; }
      //  public ICollection<UserModel> Users { get; set; }


    }
}
