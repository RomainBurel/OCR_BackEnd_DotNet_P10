using Frontend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace Frontend.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<Patient> Patients { get; set; } = new();

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync()
        {
            var token = User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                RedirectToPage("/Account/Login");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Patients = await _httpClient.GetFromJsonAsync<List<Patient>>("https://localhost:7258/Patient/list");
        }
    }
}
