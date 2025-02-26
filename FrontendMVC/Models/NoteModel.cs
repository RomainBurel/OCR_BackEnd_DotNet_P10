namespace FrontendMVC.Models
{
    public class NoteModel
    {
        public string NoteId { get; set; }

        public int PatientId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
