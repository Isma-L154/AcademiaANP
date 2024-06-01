using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ANP_Academy.DAL.Models
{
    public class IdentityContext : IdentityDbContext<Usuario>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        // Aqui se agregan los DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(x => x.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(x => x.Id);
        }
    }






}
