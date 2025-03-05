using IdentityAPI.Data;
using IdentityAPI.Domain;
using IdentityAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PatientsAPI_Test.IntegrationTests
{
    public class CustomIdentityAPIFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private const string ADMIN_NAME = "Admin";
        private const string ADMIN_MAIL = "admin@medilabo.fr";
        private const string ADMIN_PWD = "Admin@123";
        private const string ADMIN_ROLE_NAME = "Admin";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<IdentityDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                if (dbConnectionDescriptor != null)
                {
                    services.Remove(dbConnectionDescriptor);
                }

                var dbOptionsConfiguration = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<IdentityDbContext>));
                if (dbOptionsConfiguration != null)
                {
                    services.Remove(dbOptionsConfiguration);
                }

                services.AddDbContext<IdentityDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"TestIdentityDB");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    SeedAdminAndSimpleUsersAsync(scope.ServiceProvider).GetAwaiter().GetResult();
                }
            });

            builder.UseEnvironment("Development");
        }

        public async Task LoginAsAdmin(HttpClient httpClientIdentity, HttpClient httpClientPatient)
        {
            await AuthenticateUserAsync(httpClientIdentity, httpClientPatient, ADMIN_NAME, ADMIN_PWD);
        }

        public void Logout(HttpClient httpClientIdentity, HttpClient httpClientPatient)
        {
            httpClientIdentity.DefaultRequestHeaders.Authorization = null;
            httpClientPatient.DefaultRequestHeaders.Authorization = null; 
        }

        private async Task AuthenticateUserAsync(HttpClient httpClientIdentity, HttpClient httpClientPatient, string userName, string pwd)
        {
            var authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync(httpClientIdentity, userName, pwd));
            httpClientIdentity.DefaultRequestHeaders.Authorization = authorization;
            httpClientPatient.DefaultRequestHeaders.Authorization = authorization;
        }

        private async Task<string> GetJwtAsync(HttpClient httpClientIdentity, string userName, string pwd)
        {
            var loginModel = new LoginModel
            {
                Username = userName,
                Password = pwd
            };

            var response = await httpClientIdentity.PostAsJsonAsync("/api/Authorization/login", loginModel);
            response.EnsureSuccessStatusCode();

            var registrationResponse = await response.Content.ReadAsStringAsync();
            using (var document = JsonDocument.Parse(registrationResponse))
            {
                var json = document.RootElement.GetProperty("token").GetString();
                return json;
            }
        }

        public static async Task SeedAdminAndSimpleUsersAsync(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<IdentityDbContext>();
            db.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await AddRole(roleManager, ADMIN_ROLE_NAME);

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await AddUser(userManager, ADMIN_MAIL, ADMIN_NAME, ADMIN_PWD, ADMIN_ROLE_NAME);
        }

        private static async Task AddRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var identityRole = new IdentityRole(role);
                var roleResult = await roleManager.CreateAsync(identityRole);

                if (!roleResult.Succeeded)
                {
                    throw new Exception("Failed to create '" + role + "' role");
                }
            }
        }

        private static async Task AddUser(UserManager<ApplicationUser> userManager, string userMail, string userName, string pwd, string role)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userMail,
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(user, pwd);

                if (!userResult.Succeeded)
                {
                    throw new Exception("Failed to create '" + userName + "' user");
                }

                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
