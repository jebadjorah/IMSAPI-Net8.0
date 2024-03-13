using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMSAPI.Models.Administration
{
    [Table ("Tbl_Entity")]
    public class EntityModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EntityName { get; set; }
        [Required]
        public string EntityCode { get; set; }
        public bool IsActive { get; set; }
        public int ParentId { get; set; }
        public int CompanyId { get; set; }
        public CompanyModel Company { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
