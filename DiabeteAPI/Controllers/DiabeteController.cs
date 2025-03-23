using DiabeteAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiabeteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DiabeteController : Controller
    {
        private readonly ILogger<DiabeteController> _logger;
        private readonly IDiabeteService _diabeteService;
        private readonly HttpClient _httpClient;

        public DiabeteController(ILogger<DiabeteController> logger, IDiabeteService diabeteService, HttpClient httpClient)
        {
            _logger = logger;
            _diabeteService = diabeteService;
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("diabeteReport/{patientId}")]
        public async Task<IActionResult> GetDiabeteReportById(int patientId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _logger.LogInformation($"{nameof(GetDiabeteReportById)} : called with patientId {patientId}");
            var result = await _diabeteService.GetPatientDiabeteRiskReport(patientId, token);

            return Ok(result);
        }
    }
}
