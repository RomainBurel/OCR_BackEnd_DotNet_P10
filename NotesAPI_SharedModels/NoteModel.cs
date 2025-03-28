﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NotesAPI_SharedModels
{
    public class NoteModel
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

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; }
    }
}
