using PatientsAPI.Data;
using PatientsAPI.Domain;

namespace PatientsAPI.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
