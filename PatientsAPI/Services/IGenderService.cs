using PatientsAPI_SharedModels;

namespace PatientsAPI.Services
{
    public interface IGenderService
    {
        public Task<IEnumerable<GenderModel>> GetAll();
    }
}
