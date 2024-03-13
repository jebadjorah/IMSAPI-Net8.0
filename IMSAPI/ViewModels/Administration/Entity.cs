namespace IMSAPI.ViewModels.Administration
{
    public class Entity
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string EntityCode { get; set; }
        public bool IsActive { get; set; }
        public int ParentId { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
        public int? CreatedBy { get; set; }
    }
}
