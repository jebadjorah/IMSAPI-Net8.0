using IMSAPI.Filters;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;

namespace IMSAPI.Installer
{
    public class InstallerExtenstions
    {
        public static void IntallServices(WebApplicationBuilder builder, IConfigurationSection iAppSeetings = null)
        {
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IEntityService, EntityService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRolePrivilageService, RolePrivilageService>();
            builder.Services.AddScoped<TokenService, TokenService>();
            builder.Services.AddScoped<CustomAuthorizationFilter, CustomAuthorizationFilter>();
        }

    }
}
