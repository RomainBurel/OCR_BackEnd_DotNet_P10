namespace FrontendMVC.Models.Entities
{
    public class Note
    {
        public string NoteId { get; set; }

        public int PatientId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
