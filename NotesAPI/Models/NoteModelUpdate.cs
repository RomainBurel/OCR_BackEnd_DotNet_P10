using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NotesAPI.Models
{
    public class NoteModelUpdate
    {
        [BsonRepresentation(BsonType.Int32)]
        public int PatientId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; }
    }
}
