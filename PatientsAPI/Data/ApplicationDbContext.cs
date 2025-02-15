using Microsoft.EntityFrameworkCore;
using PatientsAPI.Domain;

namespace PatientsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Gender> Genders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ajout de valeurs par défaut pour Gender
            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, GenderName = "Male" },
                new Gender { Id = 2, GenderName = "Female" }
            );
        }
    }
}
