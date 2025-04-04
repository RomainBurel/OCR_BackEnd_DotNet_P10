﻿using static DiabeteAPI.Services.DiabeteService;

namespace DiabeteAPI.Services
{
    public interface IDiabeteAlgoService
    {
        public DiabeteRisk GetDiabeteRisk(PatientGender gender, int age, int nbTriggers);

        public int GetNbTriggers(List<string> patientNotesContent);

        public int CalculateAge(DateTime dateOfBirth);
    }
}
