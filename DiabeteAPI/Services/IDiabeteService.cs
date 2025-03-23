namespace DiabeteAPI.Services
{
    public interface IDiabeteService
    {
        public Task<string> GetPatientDiabeteRiskReport(int patientId, string token);
    }
}
