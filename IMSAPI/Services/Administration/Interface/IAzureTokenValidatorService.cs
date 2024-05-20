namespace IMSAPI.Services.Administration.Interface
{
    public interface IAzureTokenValidatorService
    {
        Task<bool> ValidateAzureToken(string token);
        Task<string> GetUserInfoAsync(string jwtToken);
    }
}
