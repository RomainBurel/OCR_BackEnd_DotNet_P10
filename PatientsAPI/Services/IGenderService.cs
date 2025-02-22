using PatientsAPI.Models;

namespace PatientsAPI.Services
{
    public interface IGenderService
    {
        public Task<IEnumerable<GenderModel>> GetAll();
    }
}
