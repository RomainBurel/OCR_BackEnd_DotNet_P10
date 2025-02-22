namespace FrontendMVC.Models.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}