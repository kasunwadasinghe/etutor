using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ETutor.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("User_tbl");
            modelBuilder.Entity<ApplicationUser>().ToTable("User_tbl");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole_tbl");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins_tbl");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims_tbl");
            modelBuilder.Entity<IdentityRole>().ToTable("Role_tbl");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}