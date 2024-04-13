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
    
    public class ControllersDto
    {
        public string ControllerName  { get; set; }
        public List<ActionsDto> Actions { get; set; }
    }
    public class ActionsDto
    {
        public string ActionName { get; set; }
    }
}
