using NotesAPI_SharedModels;
using PatientsAPI_SharedModels;
using System.Net.Http.Headers;

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

        private readonly HttpClient _httpClientPatients;
        private readonly HttpClient _httpClientNotes;
        private readonly IHttpClientFactory _httpClientFactoryPatientsAPI;
        private readonly IHttpClientFactory _httpClientFactoryNotesAPI;

        private readonly List<string> SearchedTriggers;

        public DiabeteService(IHttpClientFactory httpClientFactoryPatientsAPI, IHttpClientFactory httpClientFactoryNotesAPI)
        {
            _httpClientFactoryPatientsAPI = httpClientFactoryPatientsAPI;
            _httpClientFactoryNotesAPI = httpClientFactoryNotesAPI;
            _httpClientPatients = _httpClientFactoryPatientsAPI.CreateClient();
            _httpClientNotes = _httpClientFactoryNotesAPI.CreateClient();

            SearchedTriggers = new List<string>
                {
                    "Hémoglobine A1C",
                    "Microalbumine",
                    "Taille",
                    "Poids",
                    "Fumeur",
                    "Fumeuse",
                    "Anormal",
                    "Cholestérol",
                    "Vertiges",
                    "Rechute",
                    "Réaction",
                    "Anticorps"
                };
        }

        public async Task<string> GetPatientDiabeteRiskReport(int patientId, string token)
        {
            _httpClientPatients.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClientNotes.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await GetPatient(patientId);
            var age = CalculateAge(patient.DateOfBirth);
            var notesContent = await GetPatientNotesContent(patientId);
            int nbTriggers = GetNbTriggers(notesContent);
            var diabeteRisk = GetDiabeteRisk(patient.GenderId, age, nbTriggers);

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

        private async Task<PatientModel?> GetPatient(int patientId)
        {
            var patient = await _httpClientPatients.GetFromJsonAsync<PatientModel>($"https://localhost:7242/Patient/display/{patientId}");

            return patient;
        }

        private async Task<List<string>> GetPatientNotesContent(int patientId)
        {
            var notes = await _httpClientNotes.GetFromJsonAsync<List<NoteModel>>($"https://localhost:7148/Note/displayPatientNotes/{patientId}");
            var patientNotesUCase = new List<string>();
            if (notes != null)
            {
                patientNotesUCase = notes!.Select(n => n.Content).ToList();
            }

            return patientNotesUCase;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            };

            return age;
        }

        public int GetNbTriggers(List<string> patientNotesContent)
        {
            var triggersFound = new Dictionary<string, bool>();
            SearchedTriggers.ForEach(t => {
                triggersFound.Add(t, patientNotesContent.Any(n => n.Contains(t.ToUpper(), StringComparison.CurrentCultureIgnoreCase)));
            });

            return triggersFound.Count(t => t.Value);
        }

        public DiabeteRisk GetDiabeteRisk(int gender, int age, int nbTriggers)
        {
            if (age < 30)
            {
                if (gender == 1)
                {
                    if (nbTriggers >= 3 && nbTriggers <= 4) return DiabeteRisk.InDanger;
                    if (nbTriggers >= 5) return DiabeteRisk.EarlyOnset;
                }
                else
                {
                    if (nbTriggers >= 4 && nbTriggers <= 6) return DiabeteRisk.InDanger;
                    if (nbTriggers >= 7) return DiabeteRisk.EarlyOnset;
                }
            }
            else
            {
                if (nbTriggers >= 2 && nbTriggers <= 5) return DiabeteRisk.BorderLine;
                if (nbTriggers >= 6 && nbTriggers <= 7) return DiabeteRisk.InDanger;
                if (nbTriggers >= 8) return DiabeteRisk.EarlyOnset;
            }

            return DiabeteRisk.None;
        }
    }
}
