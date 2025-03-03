using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NotesAPI.Models
{
    public class NoteModelUpdate
    {
        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; }
    }
}
