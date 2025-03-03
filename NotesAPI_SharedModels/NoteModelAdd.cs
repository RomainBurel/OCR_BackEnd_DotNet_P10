using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NotesAPI_SharedModels
{
    public class NoteModelAdd
    {
        [BsonRepresentation(BsonType.Int32)]
        public int PatientId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; }
    }
}
