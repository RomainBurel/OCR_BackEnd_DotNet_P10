namespace DiabeteAPI.Services
{
    public class DiabeteService : IDiabeteService
    {
        public enum DiabeteRisk
        {
            None = 0,
            BorderLine = 1,
            InDanger = 2,
            EarlyOnset = 3
        }

        private readonly IDiabeteAlgoService _diabeteAlgoService;
        private readonly IDiabeteDataService _diabeteDataService;

        public DiabeteService(IDiabeteAlgoService diabeteAlgoService, IDiabeteDataService diabeteDataService)
        {
            _diabeteAlgoService = diabeteAlgoService;
            _diabeteDataService = diabeteDataService;
        }

        public async Task<string> GetPatientDiabeteRiskReport(int patientId, string token)
        {
            var patient = await _diabeteDataService.GetPatient(patientId, token);
            var age = _diabeteAlgoService.CalculateAge(patient.DateOfBirth);
            var notesContent = await _diabeteDataService.GetPatientNotesContent(patientId, token);
            int nbTriggers = _diabeteAlgoService.GetNbTriggers(notesContent);
            var diabeteRisk = _diabeteAlgoService.GetDiabeteRisk((PatientGender)patient.GenderId, age, nbTriggers);

            string report;
            switch (diabeteRisk)
            {
                case DiabeteRisk.None:
                    report = "Aucun risque";
                    break;
                case DiabeteRisk.BorderLine:
                    report = "Risque limité";
                    break;
                case DiabeteRisk.InDanger:
                    report = "Danger";
                    break;
                case DiabeteRisk.EarlyOnset:
                    report = "Apparition précoce";
                    break;
                default:
                    report = "Risque inconnu";
                    break;
            }

            return report;
        }
    }
}
