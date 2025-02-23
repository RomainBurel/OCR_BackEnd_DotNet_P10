using PatientsAPI.Domain;
using PatientsAPI.Models;
using PatientsAPI.Repositories;

namespace PatientsAPI.Services
{
    public class GenderService : IGenderService
    {
        private readonly IGenderRepository _genderRepository;

        public GenderService(IGenderRepository genderRpository)
        {
            this._genderRepository = genderRpository;
        }

        public async Task<IEnumerable<GenderModel>> GetAll()
        {
            var genders = await this._genderRepository.GetAll();
            return genders.Select(p => this.GetModelFromData(p));
        }

        private GenderModel GetModelFromData(Gender gender)
        {
            return new GenderModel()
            {
                Id = gender.Id,
                GenderName = gender.GenderName
            };
        }
    }
}
