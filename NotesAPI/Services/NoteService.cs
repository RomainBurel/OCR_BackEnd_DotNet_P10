using NotesAPI.Domain;
using NotesAPI.Repositories;
using NotesAPI_SharedModels;

namespace NotesAPI.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            this._noteRepository = noteRepository;
        }

        public async Task Add(NoteModelAdd newNote)
        {
            await this._noteRepository.Add(this.GetDataFromModelAdd(newNote));
        }

        public async Task Delete(string id)
        {
            await this._noteRepository.Delete(id);
        }

        public async Task DeleteAllForAPatient(int patientId)
        {
            await this._noteRepository.DeleteAllForAPatient(patientId);
        }

        public async Task<bool> Exists(string id)
        {
            return await this._noteRepository.Exists(id);
        }

        public async Task<IEnumerable<NoteModel>> GetAllForAPatient(int patientId)
        {
            var notes = await this._noteRepository.GetAllForAPatient(patientId);
            return notes.Select(p => this.GetModelFromData(p));
        }

        public async Task<NoteModel?> GetById(string id)
        {
            var note = await this._noteRepository.GetById(id);
            return note != null ? this.GetModelFromData(note) : null;
        }

        public async Task<NoteModelUpdate?> GetByIdForUpdate(string id)
        {
            var note = await this._noteRepository.GetById(id);
            return note != null ? this.GetModelUpdateFromData(note) : null;
        }

        public async Task Update(string id, NoteModelUpdate updatedNote)
        {
            var note = await this.GetDataFromModelUpdate(id, updatedNote);
            await this._noteRepository.Update(id, note);
        }

        private NoteModel GetModelFromData(Note note)
        {
            return new NoteModel()
            {
                NoteId = note.NoteId, 
                PatientId = note.PatientId,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        private NoteModelUpdate GetModelUpdateFromData(Note note)
        {
            return new NoteModelUpdate()
            {
                Content = note.Content,
            };
        }

        private Note GetDataFromModelAdd(NoteModelAdd modelAdd)
        {
            var creationDate = DateTime.Now;

            return new Note()
            {
                PatientId = modelAdd.PatientId,
                Content = modelAdd.Content,
                CreatedAt = creationDate,
                UpdatedAt = creationDate
            };
        }

        private async Task<Note> GetDataFromModelUpdate(string id, NoteModelUpdate modelUpdate)
        {
            var noteModel = await this._noteRepository.GetById(id);
            noteModel.Content = modelUpdate.Content;
            noteModel.UpdatedAt = DateTime.Now;
            return noteModel;
        }
    }
}
