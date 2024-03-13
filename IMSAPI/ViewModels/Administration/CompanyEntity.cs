namespace IMSAPI.ViewModels.Administration
{
    public class CompanyEntity
    {
        public int Id { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int? CreatedBy { get; set; }
    }
}
