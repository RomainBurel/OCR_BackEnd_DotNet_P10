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

        public async Task<IActionResult> Index()
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patients = await this._httpClient.GetFromJsonAsync<List<Patient>>("https://localhost:7258/Patient/list");
            return View(patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await this._httpClient.GetFromJsonAsync<Patient>($"https://localhost:7258/Patient/display/{id}");
            return View(patient);
        }

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

        public async Task<IActionResult> Update(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var patient = await this._httpClient.GetFromJsonAsync<PatientModelUpdate>($"https://localhost:7258/Patient/Update/{id}");
            var genders = await this._httpClient.GetFromJsonAsync<List<Gender>>("https://localhost:7258/Gender/list");
            ViewBag.PatientId = id;
            ViewBag.Genders = genders;

            return View(patient);
        }

        [HttpPost]
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

        [HttpPost]
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
    }
}
