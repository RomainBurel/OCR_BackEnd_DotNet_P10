using FrontendMVC.Models;
using NotesAPI_SharedModels;
using PatientsAPI_SharedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FrontendMVC.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly HttpClient _httpClient;

        public PatientsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patients = await _httpClient.GetFromJsonAsync<List<PatientModel>>("https://localhost:7258/Patient/list");
            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await _httpClient.GetFromJsonAsync<PatientModel>($"https://localhost:7258/Patient/display/{id}");
            var notes = await _httpClient.GetFromJsonAsync<List<NoteModel>>($"https://localhost:7258/Notes/displayPatientNotes/{id}");
            var diabeteRiskReport = await _httpClient.GetStringAsync($"https://localhost:7258/Diabete/diabeteReport/{id}");
            var patientDetailsViewModel = new PatientDetailsViewModel { Patient = patient, Notes = notes, DiabeteRiskReport = diabeteRiskReport };

            return View(patientDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var genders = await _httpClient.GetFromJsonAsync<List<GenderModel>>("https://localhost:7258/Gender/list");
            ViewBag.Genders = genders;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientModelAdd patient)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7258/Patient/creation", patient);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de l'ajout du patient.");
                return View(patient);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await _httpClient.GetFromJsonAsync<PatientModelUpdate>($"https://localhost:7258/Patient/Update/{id}");
            var genders = await _httpClient.GetFromJsonAsync<List<GenderModel>>("https://localhost:7258/Gender/list");
            ViewBag.PatientId = id;
            ViewBag.Genders = genders;

            return View(patient);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] int id, [FromForm] PatientModelUpdate patient)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7258/Patient/update/{id}", patient);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la modification du patient.");
                return View(patient);
            }

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"https://localhost:7258/Patient/deletion/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la suppression du patient.");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(int Id, string Content)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var newNote = new NoteModelAdd
            {
                PatientId = Id,
                Content = Content
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7258/Notes/creation", newNote);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de l'ajout de la note.");
            }

            return RedirectToAction("Details", new { Id = Id });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateNote(string noteId, int patientId)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var note = await _httpClient.GetFromJsonAsync<NoteModelUpdate>($"https://localhost:7258/Notes/update/{noteId}");

            if (note == null)
            {
                return NotFound();
            }

            ViewBag.NoteId = noteId;
            ViewBag.PatientId = patientId;

            return View(note);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNote([FromForm] string id, [FromForm] int patientId, [FromForm] NoteModelUpdate model)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7258/Notes/update/{id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la modification de la note.");
                return View(model);
            }

            return RedirectToAction("Details", new { Id = patientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNote([FromForm] string id, [FromForm] int patientId)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"https://localhost:7258/Notes/deletion/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la suppression de la note.");
            }

            return RedirectToAction("Details", new { Id = patientId });
        }
    }
}
