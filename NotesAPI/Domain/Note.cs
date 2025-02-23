using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NotesAPI.Domain
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string NoteId { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int PatientId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
