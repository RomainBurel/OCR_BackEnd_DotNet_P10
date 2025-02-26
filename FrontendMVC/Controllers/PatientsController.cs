using FrontendMVC.Models;
using FrontendMVC.Models.Entities;
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
            this._httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patients = await this._httpClient.GetFromJsonAsync<List<Patient>>("https://localhost:7258/Patient/list");
            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await this._httpClient.GetFromJsonAsync<Patient>($"https://localhost:7258/Patient/display/{id}");
            var notes = await _httpClient.GetFromJsonAsync<List<Note>>($"https://localhost:7258/Notes/displayPatientNotes/{id}");

            var patientDetailsViewModel = new PatientDetailsViewModel { Patient = patient, Notes = notes };

            return View(patientDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = User.FindFirst("Token")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var genders = await this._httpClient.GetFromJsonAsync<List<Gender>>("https://localhost:7258/Gender/list");
            ViewBag.Genders = genders;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientModelAdd patient)
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await this._httpClient.PostAsJsonAsync("https://localhost:7258/Patient/creation", patient);

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
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await this._httpClient.GetFromJsonAsync<PatientModelUpdate>($"https://localhost:7258/Patient/Update/{id}");
            var genders = await this._httpClient.GetFromJsonAsync<List<Gender>>("https://localhost:7258/Gender/list");
            var notes = await _httpClient.GetFromJsonAsync<List<Note>>($"https://localhost:7258/Notes/displayPatientNotes/{id}");
            ViewBag.PatientId = id;
            ViewBag.Genders = genders;

            var patientModelUpdate = new PatientModelUpdate
            {
                PatientId = id,
                LastName = patient.LastName,
                FirstName = patient.FirstName,
                DateOfBirth = DateTime.Now,
                GenderId = patient.GenderId,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                Notes = notes
            };

            return View(patientModelUpdate);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] int id, [FromForm] PatientModelUpdate patient)
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7258/Patient/update/{id}", patient);

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

            return RedirectToAction("Update", new { Id = Id });
        }
    }
}
