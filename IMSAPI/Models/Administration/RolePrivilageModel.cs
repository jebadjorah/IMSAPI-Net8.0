using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSAPI.Models.Administration
{
    [Table("Tbl_RolePrivilage")]
    public class RolePrivilageModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ControllerName { get; set; }
        [Required]
        public string ActionName { get; set; }
        [Required]
        public int RoleId { get; set; }
        public bool? IsAllowed { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }

}
