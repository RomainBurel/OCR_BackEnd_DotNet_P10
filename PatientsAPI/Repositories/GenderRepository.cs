using PatientsAPI.Data;
using PatientsAPI.Domain;

namespace PatientsAPI.Repositories
{
    public class GenderRepository : GenericRepository<Gender>, IGenderRepository
    {
        public GenderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
