using IMSAPI.Models.Administration;
using Microsoft.EntityFrameworkCore;

namespace IMSAPI.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CompanyModel> companyModels { get; set; }
        public DbSet<EntityModel> entityModels { get; set; }
        public DbSet<RoleModel> roleModels { get; set; }
        public DbSet<UserModel> userModels { get; set; }
        public DbSet<RolePrivilageModel> rolePrivilageModels { get; set; }
    }
}
