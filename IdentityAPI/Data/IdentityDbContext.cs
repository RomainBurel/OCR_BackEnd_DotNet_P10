using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityAPI.Domain;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Data
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "9b69b13c-1e84-4c38-9f56-54d24c7a54d1";
            var adminUserId = "7e8823cd-4031-4a84-8f5f-c241b6a7d763";

            // Création du rôle administrateur
            var adminRole = new IdentityRole("Admin")
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
            builder.Entity<IdentityRole>().HasData(adminRole);

            // Création de l'utilisateur administrateur
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@medilabo.com",
                NormalizedEmail = "ADMIN@MEDILABO.COM",
                EmailConfirmed = true,
                SecurityStamp = "STATIC_SECURITY_STAMP",
                ConcurrencyStamp = "STATIC_CONCURRENCY_STAMP"
            };

            // Hachage du mot de passe
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Assignation du rôle Admin à l'utilisateur
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            });
        }
    }
}