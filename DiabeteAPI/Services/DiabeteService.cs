namespace DiabeteAPI.Services
{
    public class DiabeteService : IDiabeteService
    {
        public enum DiabeteRisk
        {
            None = 0,
            BorderLine = 1,
            InDanger = 2,
            EarlyOnset = 3
        }

        public DiabeteRisk GetDiabeteRisk(int gender, int age, int nbTriggers)
        {
            // Les règles pour déterminer les niveaux de risque sont les suivantes :
            // ● aucun risque (None) : Le dossier du patient ne contient aucune note du médecin
            //   contenant les déclencheurs(terminologie);
            // ● risque limité (Borderline) : Le dossier du patient contient entre deux et cinq
            //   déclencheurs et le patient est âgé de plus de 30 ans ;
            // ● danger (In Danger) : Dépend de l'âge et du sexe du patient.
            //   Si le patient est un homme de moins de 30 ans, alors trois termes déclencheurs doivent être présents.
            //   Si le patient est une femme et a moins de 30 ans, il faudra quatre termes déclencheurs.
            //   Si le patient a plus de 30 ans, alors il en faudra six ou sept ;
            // ● apparition précoce (Early onset) : Encore une fois, cela dépend de l'âge et du sexe.
            //   Si le patient est un homme de moins de 30 ans, alors au moins cinq termes déclencheurs sont nécessaires.
            //   Si le patient est une femme et a moins de 30 ans, il faudra au moins sept termes déclencheurs.
            //   Si le patient a plus de 30 ans, alors il en faudra huit ou plus
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
