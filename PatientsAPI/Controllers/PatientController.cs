using Microsoft.AspNetCore.Mvc;
using PatientsAPI.Models;
using PatientsAPI.Services;

namespace PatientsAPI.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientService _patientService;

        public PatientController(ILogger<PatientController> logger, IPatientService patientService)
        {
            this._logger = logger;
            this._patientService = patientService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetAllPatients()
        {
            this._logger.LogInformation($"{nameof(GetAllPatients)} requested");
            var result = await this._patientService.GetAll();

            return Ok(result);
        }

        [HttpGet]
        [Route("display/{patientId}")]
        public async Task<IActionResult> GetPatientById(int patientId)
        {
            this._logger.LogInformation($"{nameof(GetPatientById)} : called with patientId {patientId}");
            var result = await this._patientService.GetById(patientId);

            if (result == null)
            {
                _logger.LogWarning($"Patient with id {patientId} not found");
                return NotFound($"Patient with id {patientId} not found");
            }

            _logger.LogInformation($"Patient with id {patientId} not found");
            return Ok(result);
        }

        [HttpPost]
        [Route("creation")]
        public async Task<IActionResult> AddPatient([FromBody] PatientModelAdd patientModelAdd)
        {
            this._logger.LogInformation("Patient add requested");
            await this._patientService.Add(patientModelAdd);
            this._logger.LogInformation("Patient add successfull");

            return Ok();
        }

        [HttpPut]
        [Route("update/{patientId}")]
        public async Task<IActionResult> UpdatePatient(int patientId, [FromBody] PatientModelUpdate patientModelUpdate)
        {
            this._logger.LogInformation("Patient update requested");

            var patientExists = await this._patientService.Exists(patientId);
            if (!patientExists)
            {
                this._logger.LogWarning($"Patient with id {patientId} not found for update");
                return NotFound($"Patient with id {patientId} not found for update");
            }

            await this._patientService.Update(patientId, patientModelUpdate);
            this._logger.LogInformation("Patient update successfull");

            return Ok();
        }

        [HttpDelete]
        [Route("deletion/{patientId}")]
        public async Task<IActionResult> DeletePatient(int patientId)
        {
            this._logger.LogInformation("Patient delete requested");

            var patientExists = await this._patientService.Exists(patientId);
            if (!patientExists)
            {
                this._logger.LogWarning($"Patient with id {patientId} not found for deletion");
                return NotFound($"Patient with id {patientId} not found for deletion");
            }

            await this._patientService.Delete(patientId);
            this._logger.LogInformation("Patient delete successfull");

            return Ok();
        }
    }
}
