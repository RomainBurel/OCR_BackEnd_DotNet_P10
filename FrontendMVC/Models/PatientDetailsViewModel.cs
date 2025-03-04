using NotesAPI_SharedModels;
using PatientsAPI_SharedModels;

namespace FrontendMVC.Models
{
    public class PatientDetailsViewModel
    {
        public PatientModel Patient { get; set; }
        public List<NoteModel> Notes { get; set; }
    }
}
