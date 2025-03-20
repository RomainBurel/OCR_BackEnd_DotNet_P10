using NotesAPI_SharedModels;
using PatientsAPI_SharedModels;
using System.Net.Http.Headers;

namespace DiabeteAPI.Services
{
    public class DiabeteDataService : IDiabeteDataService
    {
        private readonly HttpClient _httpClientPatients;
        private readonly HttpClient _httpClientNotes;
        private readonly IHttpClientFactory _httpClientFactoryPatientsAPI;
        private readonly IHttpClientFactory _httpClientFactoryNotesAPI;
        private readonly IConfiguration _configuration;

        public DiabeteDataService(IHttpClientFactory httpClientFactoryPatientsAPI, IHttpClientFactory httpClientFactoryNotesAPI, IConfiguration configuration)
        {
            _httpClientFactoryPatientsAPI = httpClientFactoryPatientsAPI;
            _httpClientFactoryNotesAPI = httpClientFactoryNotesAPI;
            _httpClientPatients = _httpClientFactoryPatientsAPI.CreateClient();
            _httpClientNotes = _httpClientFactoryNotesAPI.CreateClient();
            _configuration = configuration;
        }

        public async Task<PatientModel?> GetPatient(int patientId, string token)
        {
            _httpClientPatients.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var patientsApiUrl = _configuration["ApiURLs:patientsApiURL"];
            var patient = await _httpClientPatients.GetFromJsonAsync<PatientModel>($"{patientsApiUrl}/Patient/display/{patientId}");

            return patient;
        }

        public async Task<List<string>> GetPatientNotesContent(int patientId, string token)
        {
            _httpClientNotes.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var notesApiUrl = _configuration["ApiURLs:notesApiURL"];
            var notes = await _httpClientNotes.GetFromJsonAsync<List<NoteModel>>($"{notesApiUrl}/Note/displayPatientNotes/{patientId}");
            var patientNotesUCase = new List<string>();
            if (notes != null)
            {
                patientNotesUCase = notes!.Select(n => n.Content).ToList();
            }

            return patientNotesUCase;
        }
    }
}
