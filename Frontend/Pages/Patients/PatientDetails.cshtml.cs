using Frontend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace Frontend.Pages.Patients
{
    public class PatientDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public Patient Patient { get; set; }

        public PatientDetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync(int id)
        {
            var token = User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                RedirectToPage("/Account/Login");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Patient = await _httpClient.GetFromJsonAsync<Patient>($"https://localhost:7258/Patient/display/{id}");
        }
    }
}
