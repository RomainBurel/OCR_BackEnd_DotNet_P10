namespace FrontendMVC.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using System.Threading.Tasks;

    namespace FrontendMVC.Controllers
    {
        public class AccountController : Controller
        {
            private readonly HttpClient _httpClient;

            public AccountController(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public IActionResult Login() => View();

            [HttpPost]
            public async Task<IActionResult> Login(string username, string password)
            {
                var response = await this._httpClient.PostAsJsonAsync("https://localhost:7258/Authorization/login", new { username, password });

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Login failed. Check your credentials.";
                    return View();
                }

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("Token", tokenResponse.Token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Patients");
            }

            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login");
            }

            private class TokenResponse
            {
                public string Token { get; set; }
            }
        }
    }
}
