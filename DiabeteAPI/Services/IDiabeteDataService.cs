using PatientsAPI_SharedModels;

namespace DiabeteAPI.Services
{
    public interface IDiabeteDataService
    {
        public Task<PatientModel?> GetPatient(int patientId, string token);

        public Task<List<string>> GetPatientNotesContent(int patientId, string token);
    }
}
