namespace IMSAPI.ViewModels.Administration
{
    public class RolePrivilageEntity
    {
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int RoleId { get; set; }
        public bool? IsAllowed { get; set; }
    }
}
