using NotesAPI.Domain;

namespace NotesAPI.Repositories
{
    public interface INoteRepository
    {
        public Task Add(Note newNote);

        public Task Delete(string id);

        public Task DeleteAllForAPatient(int patientId);

        public Task<bool> Exists(string id);

        public Task<IEnumerable<Note>> GetAllForAPatient(int patientId);

        public Task<Note> GetById(string id);

        public Task Update(string id, Note updatedNote);
    }
}
