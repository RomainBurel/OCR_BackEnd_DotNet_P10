using PatientsAPI.Domain;
using PatientsAPI_SharedModels;
using PatientsAPI.Repositories;

namespace PatientsAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository;
        }

        public async Task<PatientModel?> GetById(int id)
        {
            var patient = await this._patientRepository.GetById(id);
            return patient != null ? this.GetModelFromData(patient) : null;
        }

        public async Task<PatientModelUpdate?> GetByIdForUpdate(int id)
        {
            var patient = await this._patientRepository.GetById(id);
            return patient != null ? this.GetModelUpdateFromData(patient) : null;
        }

        public async Task<IEnumerable<PatientModel>> GetAll()
        {
            var patients = await this._patientRepository.GetAll();
            return patients.Select(p => this.GetModelFromData(p));
        }

        public async Task<bool> Exists(int id)
        {
            return await this._patientRepository.Exists(id);
        }

        public async Task Add(PatientModelAdd modelAdd)
        {
            await this._patientRepository.Add(this.GetDataFromModelAdd(modelAdd));
        }

        public async Task Update(int id, PatientModelUpdate modelUpdate)
        {
            var patient = await this.GetDataFromModelUpdate(id, modelUpdate);
            await this._patientRepository.Update(patient);
        }

        public async Task Delete(int id)
        {
            await this._patientRepository.Delete(id);
        }

        private PatientModel GetModelFromData(Patient patient)
        {
            return new PatientModel()
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                GenderId = patient.GenderId,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        private PatientModelUpdate GetModelUpdateFromData(Patient patient)
        {
            return new PatientModelUpdate()
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                GenderId = patient.GenderId,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber
            };
        }

        private Patient GetDataFromModelAdd(PatientModelAdd modelAdd)
        {
            var creationDate = DateTime.Now;
            return new Patient()
            {
                FirstName = modelAdd.FirstName,
                LastName = modelAdd.LastName,
                DateOfBirth = modelAdd.DateOfBirth,
                GenderId = modelAdd.GenderId,
                Address = modelAdd.Address,
                PhoneNumber = modelAdd.PhoneNumber,
                CreatedAt = creationDate,
                UpdatedAt = creationDate
            };
        }

        private async Task<Patient> GetDataFromModelUpdate(int id, PatientModelUpdate modelUpdate)
        {
            var patientModel = await this._patientRepository.GetById(id);
            patientModel.FirstName = modelUpdate.FirstName;
            patientModel.LastName = modelUpdate.LastName;
            patientModel.DateOfBirth = modelUpdate.DateOfBirth;
            patientModel.GenderId = modelUpdate.GenderId;
            patientModel.Address = modelUpdate.Address;
            patientModel.PhoneNumber = modelUpdate.PhoneNumber;
            patientModel.UpdatedAt = DateTime.Now;
            return patientModel;
        }
    }
}
