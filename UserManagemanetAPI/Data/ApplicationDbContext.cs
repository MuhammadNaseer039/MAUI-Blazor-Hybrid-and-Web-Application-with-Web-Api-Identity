using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UserManagementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    AddRole(builder);
        //}

        //private void AddRole(ModelBuilder builder)
        //{
        //    builder.Entity<IdentityRole>().HasData
        //        (
        //            new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
        //            new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" },
        //            new IdentityRole() { Name = "HR", ConcurrencyStamp = "3", NormalizedName = "HR" }
        //        );
        //}
    }
}
