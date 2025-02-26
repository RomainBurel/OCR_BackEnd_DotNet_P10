using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Domain;
using NotesAPI.Models;
using NotesAPI.Services;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            this._noteService = noteService;
        }

        [HttpGet]
        [Route("display/{noteId}")]
        public async Task<ActionResult<Note>> GetNote(string noteId)
        {
            var note = await this._noteService.GetById(noteId);
            return Ok(note);
        }

        [HttpGet]
        [Route("displayPatientNotes/{patientId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetPatientNotes(int patientId)
        {
            Console.WriteLine($"Utilisateur authentifié ? {User.Identity.IsAuthenticated}");
            Console.WriteLine($"Claims reçus : {string.Join(", ", User.Claims.Select(c => c.Type + " = " + c.Value))}");
            var notes = await this._noteService.GetAllForAPatient(patientId);
            return Ok(notes);
        }

        [HttpPost]
        [Route("creation")]
        public async Task<ActionResult> CreateNote(NoteModelAdd note)
        {
            try
            {
                await this._noteService.Add(note);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{noteId}")]
        public async Task<ActionResult> UpdateNote(string noteId, NoteModelUpdate note)
        {
            try
            {
                await this._noteService.Update(noteId, note);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deletion/{noteId}")]
        public async Task<ActionResult> DeleteNote(string noteId)
        {
            try
            {
                await this._noteService.Delete(noteId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deletionForPatient/{patientId}")]
        public async Task<ActionResult> DeletePatientNotes(int patientId)
        {
            try
            {
                await this._noteService.DeleteAllForAPatient(patientId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
