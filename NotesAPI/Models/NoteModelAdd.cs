using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NotesAPI.Models
{
    public class NoteModelAdd
    {
        [BsonRepresentation(BsonType.Int32)]
        public int PatientId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
