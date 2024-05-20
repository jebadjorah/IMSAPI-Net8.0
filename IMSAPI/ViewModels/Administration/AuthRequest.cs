namespace IMSAPI.ViewModels.Administration
{
    public class AuthRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public  DateTime? expires { get; set; }
    }
    public class ClaimResponse
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string  RoleName { get; set;}
        
    }
    public class AzureLoginRequest
    {
        public string UserName { get; set; }
        public string AzureToken { get; set; }

    }
}
