using MongoDB.Driver;
using NotesAPI.Domain;

namespace NotesAPI.Data
{
    public class NoteDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Note> Notes => this._database.GetCollection<Note>("Notes");

        public NoteDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            this._database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
        }
    }
}
