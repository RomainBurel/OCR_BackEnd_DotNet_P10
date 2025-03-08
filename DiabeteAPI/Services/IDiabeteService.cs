using static DiabeteAPI.Services.DiabeteService;

namespace DiabeteAPI.Services
{
    public interface IDiabeteService
    {
        public Task<string> GetPatientDiabeteRiskReport(int patientId, string? token);

        public DiabeteRisk GetDiabeteRisk(int gender, int age, int nbTriggers);

        public int GetNbTriggers(List<string> patientNotesContent);
    }
}
