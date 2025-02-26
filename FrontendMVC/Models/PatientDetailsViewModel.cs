using FrontendMVC.Models.Entities;

namespace FrontendMVC.Models
{
    public class PatientDetailsViewModel
    {
        public Patient Patient { get; set; }
        public List<Note> Notes { get; set; }
    }
}
