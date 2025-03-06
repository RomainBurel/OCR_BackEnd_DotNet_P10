using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotesAPI.Domain;
using NotesAPI.Models;

namespace NotesAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _notesCollection;

        public NoteRepository(IOptions<NoteDatabaseSettings> noteDatabaseSettings)
        {
            var mongoClient = new MongoClient(noteDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(noteDatabaseSettings.Value.DatabaseName);
            this._notesCollection = mongoDatabase.GetCollection<Note>(noteDatabaseSettings.Value.CollectionName);
        }

        public async Task Add(Note newNote)
        {
            await this._notesCollection.InsertOneAsync(newNote);
        }

        public async Task Delete(string id)
        {
            await this._notesCollection.DeleteOneAsync(id);
        }

        public async Task DeleteAllForAPatient(int patientId)
        {
            await this._notesCollection.DeleteManyAsync(n => n.PatientId == patientId);
        }

        public async Task<bool> Exists(string id)
        {
            var note = await this._notesCollection.Find(n => n.NoteId == id).FirstOrDefaultAsync();
            return note != null;
        }

        public async Task<IEnumerable<Note>> GetAllForAPatient(int patientId)
        {
            return await this._notesCollection.Find(n => n.PatientId == patientId).ToListAsync<Note>();
        }

        public async Task<Note> GetById(string id)
        {
            return await this._notesCollection.Find(n => n.NoteId == id).FirstOrDefaultAsync();
        }

        public async Task Update(string id, Note updatedNote)
        {
            await this._notesCollection.ReplaceOneAsync(n => n.NoteId == id, updatedNote);
        }
    }
}
