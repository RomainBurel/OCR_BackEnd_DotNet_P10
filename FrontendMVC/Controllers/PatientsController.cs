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

            var patients = await _httpClient.GetFromJsonAsync<List<PatientModel>>("http://gateway/Patient/list");
            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await _httpClient.GetFromJsonAsync<PatientModel>($"http://gateway/Patient/display/{id}");
            var notes = await _httpClient.GetFromJsonAsync<List<NoteModel>>($"http://gateway/Note/displayPatientNotes/{id}");
            var diabeteRiskReport = await _httpClient.GetStringAsync($"http://gateway/Diabete/diabeteReport/{id}");
            var patientDetailsViewModel = new PatientDetailsViewModel { Patient = patient, Notes = notes, DiabeteRiskReport = diabeteRiskReport };

            return View(patientDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var genders = await _httpClient.GetFromJsonAsync<List<GenderModel>>("http://gateway/Gender/list");
            ViewBag.Genders = genders;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientModelAdd patient)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("http://gateway/Patient/creation", patient);

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

            var patient = await _httpClient.GetFromJsonAsync<PatientModelUpdate>($"http://gateway/Patient/Update/{id}");
            var genders = await _httpClient.GetFromJsonAsync<List<GenderModel>>("http://gateway/Gender/list");
            ViewBag.PatientId = id;
            ViewBag.Genders = genders;

            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] int id, [FromForm] PatientModelUpdate patient)
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"http://gateway/Patient/update/{id}", patient);

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

            var response = await _httpClient.DeleteAsync($"http://gateway/Patient/deletion/{id}");

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

            var response = await _httpClient.PostAsJsonAsync("http://gateway/Note/creation", newNote);

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

            var note = await _httpClient.GetFromJsonAsync<NoteModelUpdate>($"http://gateway/Note/update/{noteId}");

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

            var response = await _httpClient.PutAsJsonAsync($"http://gateway/Note/update/{id}", model);

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

            var response = await _httpClient.DeleteAsync($"http://gateway/Note/deletion/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la suppression de la note.");
            }

            return RedirectToAction("Details", new { Id = patientId });
        }
    }
}
