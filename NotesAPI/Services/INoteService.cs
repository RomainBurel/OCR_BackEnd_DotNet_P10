using NotesAPI_SharedModels;

namespace NotesAPI.Services
{
    public interface INoteService
    {
        public Task Add(NoteModelAdd newNote);

        public Task Delete(string id);

        public Task DeleteAllForAPatient(int patientId);

        public Task<bool> Exists(string id);

        public Task<IEnumerable<NoteModel>> GetAllForAPatient(int patientId);

        public Task<NoteModel?> GetById(string id);

        public Task<NoteModelUpdate?> GetByIdForUpdate(string id);

        public Task Update(string id, NoteModelUpdate updatedNote);
    }
}
