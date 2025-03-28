﻿namespace PatientsAPI_SharedModels
{
    public class PatientModel
    {
        public int PatientId { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int GenderId { get; set; }

        public string? Address { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
