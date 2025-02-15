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
            base.OnModelCreating(modelBuilder);

            // Ajout de valeurs par défaut pour Gender
            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, GenderName = "Male" },
                new Gender { Id = 2, GenderName = "Female" }
            );

            // Ajout de valeurs par défaut pour Patient
            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    PatientId = 1,
                    FirstName = "Test",
                    LastName = "TestNone",
                    DateOfBirth = new DateTime(1966, 12, 31),
                    GenderId = 2,
                    Address = "1 Brookside St",
                    PhoneNumber = "100-222-3333"
                },
                new Patient
                {
                    PatientId = 2,
                    FirstName = "Test",
                    LastName = "TestBorderline",
                    DateOfBirth = new DateTime(1945, 06, 24),
                    GenderId = 1,
                    Address = "2 High St",
                    PhoneNumber = "200-333-4444"
                },
                new Patient
                {
                    PatientId = 3,
                    LastName = "TestDanger",
                    FirstName = "Test",
                    DateOfBirth = new DateTime(2004, 06, 18),
                    GenderId = 1,
                    Address = "3 Club Road",
                    PhoneNumber = "300-444-5555"
                },
                new Patient
                {
                    PatientId = 4,
                    FirstName = "Test",
                    LastName = "TestEarlyOnset",
                    DateOfBirth = new DateTime(2002, 06, 28),
                    GenderId = 2,
                    Address = "4 Valley Dr",
                    PhoneNumber = "400-555-6666"
                }
            );
        }
    }
}
