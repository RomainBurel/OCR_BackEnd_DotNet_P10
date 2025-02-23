using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientsAPI.Services;

namespace PatientsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GenderController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IGenderService _genderService;

        public GenderController(ILogger<PatientController> logger, IGenderService genderService)
        {
            this._logger = logger;
            this._genderService = genderService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetAllGenders()
        {
            this._logger.LogInformation($"{nameof(GetAllGenders)} requested");
            var result = await this._genderService.GetAll();

            return Ok(result);
        }
    }
}
