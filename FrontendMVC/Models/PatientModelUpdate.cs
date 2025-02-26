using FrontendMVC.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace FrontendMVC.Models
{
    public class PatientModelUpdate
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "La saisie du nom du patient est obligatoire")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La saisie du prénom du patient est obligatoire")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La saisie de la date de naissance du patient est obligatoire")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "La saisie du genre du patient est obligatoire")]
        public int GenderId { get; set; }

        public string? Address { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; } = string.Empty;

        public List<Note> Notes { get; set; }
    }
}
