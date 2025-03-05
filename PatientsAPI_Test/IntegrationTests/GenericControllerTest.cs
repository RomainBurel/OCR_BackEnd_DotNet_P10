using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PatientsAPI.Data;

namespace PatientsAPI_Test.IntegrationTests
{
    public class GenericControllerTest<T> : IClassFixture<CustomPatientsAPIFactory<PatientsAPI.Program>> where T : class
    {
        protected readonly CustomPatientsAPIFactory<PatientsAPI.Program> _factoryPatient;
        protected readonly HttpClient _httpClientPatient;

        public GenericControllerTest(CustomPatientsAPIFactory<PatientsAPI.Program> factoryPatient)
        {
            _factoryPatient = factoryPatient;
            _httpClientPatient = factoryPatient.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
        }

        protected async Task FillTable(List<T> records)
        {
            using (var scope = _factoryPatient.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dbSet = context.Set<T>();
                dbSet.AddRange(records);
                await context.SaveChangesAsync();
            }
        }

        protected List<T> GetAllRecordsInTable()
        {
            using (var scope = _factoryPatient.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dbSet = context.Set<T>();
                return dbSet.ToList();
            }
        }

        protected int NbRecordsInTable()
        {
            using (var scope = _factoryPatient.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dbSet = context.Set<T>();
                return dbSet.Count();
            }
        }

        protected T GetFirstRecordInTable()
        {
            using (var scope = _factoryPatient.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dbSet = context.Set<T>();
                return dbSet.First();
            }
        }

        protected T GetRecordById(object id)
        {
            using (var scope = _factoryPatient.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dbSet = context.Set<T>();
                return dbSet.Find(id);
            }
        }

        protected virtual async Task ClearTable()
        {
            using (var scope = _factoryPatient.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var dbSet = context.Set<T>();
                context.RemoveRange(dbSet.ToArray());
                await context.SaveChangesAsync();
            }
        }
    }
}
