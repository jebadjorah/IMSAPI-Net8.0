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
    }
}
