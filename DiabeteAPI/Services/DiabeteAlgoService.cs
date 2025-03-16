﻿using static DiabeteAPI.Services.DiabeteService;

namespace DiabeteAPI.Services
{
    public class DiabeteAlgoService : IDiabeteAlgoService
    {
        private readonly List<string> SearchedTriggers;

        public DiabeteAlgoService()
        {
            SearchedTriggers = new List<string>
                {
                    "Hémoglobine A1C",
                    "Microalbumine",
                    "Taille",
                    "Poids",
                    "Fumeur",
                    "Fumeuse",
                    "Anormal",
                    "Cholestérol",
                    "Vertiges",
                    "Rechute",
                    "Réaction",
                    "Anticorps"
                };
        }

        public int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            };

            return age;
        }

        public int GetNbTriggers(List<string> patientNotesContent)
        {
            var triggersFound = new Dictionary<string, bool>();
            SearchedTriggers.ForEach(t => {
                triggersFound.Add(t, patientNotesContent.Any(n => n.Contains(t.ToUpper(), StringComparison.CurrentCultureIgnoreCase)));
            });

            return triggersFound.Count(t => t.Value);
        }

        public DiabeteRisk GetDiabeteRisk(int gender, int age, int nbTriggers)
        {
            if (age < 30)
            {
                if (gender == 1)
                {
                    if (nbTriggers >= 3 && nbTriggers <= 4) return DiabeteRisk.InDanger;
                    if (nbTriggers >= 5) return DiabeteRisk.EarlyOnset;
                }
                else
                {
                    if (nbTriggers >= 4 && nbTriggers <= 6) return DiabeteRisk.InDanger;
                    if (nbTriggers >= 7) return DiabeteRisk.EarlyOnset;
                }
            }
            else
            {
                if (nbTriggers >= 2 && nbTriggers <= 5) return DiabeteRisk.BorderLine;
                if (nbTriggers >= 6 && nbTriggers <= 7) return DiabeteRisk.InDanger;
                if (nbTriggers >= 8) return DiabeteRisk.EarlyOnset;
            }

            return DiabeteRisk.None;
        }
    }
}
