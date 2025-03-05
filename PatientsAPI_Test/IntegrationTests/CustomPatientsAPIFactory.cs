using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PatientsAPI.Data;
using System.Data.Common;

namespace PatientsAPI_Test.IntegrationTests
{
    public class CustomPatientsAPIFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                if (dbConnectionDescriptor != null)
                {
                    services.Remove(dbConnectionDescriptor);
                }

                var dbOptionsConfiguration = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));
                if (dbOptionsConfiguration != null)
                {
                    services.Remove(dbOptionsConfiguration);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"TestPatientsDB");
                });

                // Add FakPolicyEvaluator to by-pass controller Authorize attribute
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                var sp = services.BuildServiceProvider();
            });

            builder.UseEnvironment("Development");
        }
    }
}
