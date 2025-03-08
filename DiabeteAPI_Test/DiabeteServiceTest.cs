using DiabeteAPI.Services;
using Moq;
using static DiabeteAPI.Services.DiabeteService;

namespace DiabeteAPI_Test
{
    public class DiabeteServiceTest
    {
        private readonly IDiabeteService _diabeteService;
        private readonly List<string> SearchedTriggers;

        #region Constructor
        public DiabeteServiceTest()
        {
            var httpClientFactoryPatientsMock = new Mock<IHttpClientFactory>();
            var httpClientFactoryNotesMock = new Mock<IHttpClientFactory>();
            _diabeteService = new DiabeteService(httpClientFactoryPatientsMock.Object, httpClientFactoryNotesMock.Object);
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
        #endregion

        #region GetDiabeteRisk
        [Fact]
        public void GetDiabeteRisk_TestCase1_ShouldReturn_RiskNone()
        {
            var woman58Risk = _diabeteService.GetDiabeteRisk(2, 58, 1);
            var expectedRisk = DiabeteRisk.None;

            Assert.Equal(expectedRisk, woman58Risk);
        }

        [Fact]
        public void GetDiabeteRisk_TestCase2_ShouldReturn_RiskBorderLine()
        {
            var man79Risk = _diabeteService.GetDiabeteRisk(1, 79, 2);
            var expectedRisk = DiabeteRisk.BorderLine;

            Assert.Equal(expectedRisk, man79Risk);
        }

        [Fact]
        public void GetDiabeteRisk_TestCase3_ShouldReturn_RiskInDanger()
        {
            var man20Risk = _diabeteService.GetDiabeteRisk(1, 20, 3);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(expectedRisk, man20Risk);
        }

        [Fact]
        public void GetDiabeteRisk_TestCase3_ShouldReturn_RiskEarlyOnset()
        {
            var woman22Risk = _diabeteService.GetDiabeteRisk(2, 22, 6);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(expectedRisk, woman22Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Patient_LessThanTwoTriggers_HasRiskNone()
        {
            var woman25Risk = _diabeteService.GetDiabeteRisk(2, 25, 1);
            var woman30Risk = _diabeteService.GetDiabeteRisk(2, 25, 0);
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 1);
            var man25Risk = _diabeteService.GetDiabeteRisk(1, 25, 1);
            var man30Risk = _diabeteService.GetDiabeteRisk(1, 25, 0);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 1);
            var expectedRisk = DiabeteRisk.None;

            Assert.Equal(expectedRisk, woman25Risk);
            Assert.Equal(expectedRisk, woman30Risk);
            Assert.Equal(expectedRisk, woman35Risk);
            Assert.Equal(expectedRisk, man25Risk);
            Assert.Equal(expectedRisk, man30Risk);
            Assert.Equal(expectedRisk, man35Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Patient_Over30_FiveTriggers_ShouldReturn_Borderline()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 5);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 5);
            var expectedRisk = DiabeteRisk.BorderLine;

            Assert.Equal(expectedRisk, woman35Risk);
            Assert.Equal(expectedRisk, man35Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Man_Under30_ThreeTriggers_ShouldReturn_InDanger()
        {
            var man25Risk = _diabeteService.GetDiabeteRisk(1, 25, 3);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(expectedRisk, man25Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Woman_Under30_FourTriggers_ShouldReturn_InDanger()
        {
            var woman25Risk = _diabeteService.GetDiabeteRisk(2, 25, 4);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(expectedRisk, woman25Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Patient_Over30_SixTriggers_ShouldReturn_InDanger()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 6);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 6);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(expectedRisk, woman35Risk);
            Assert.Equal(expectedRisk, man35Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Patient_Over30_SevenTriggers_ShouldReturn_InDanger()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 7);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 7);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(expectedRisk, woman35Risk);
            Assert.Equal(expectedRisk, man35Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Man_Under30_FiveTriggers_ShouldReturn_EarlyOnset()
        {
            var man25Risk = _diabeteService.GetDiabeteRisk(1, 25, 5);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(expectedRisk, man25Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Woman_Under30_SevenTriggers_ShouldReturn_EarlyOnset()
        {
            var woman25Risk = _diabeteService.GetDiabeteRisk(2, 25, 7);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(expectedRisk, woman25Risk);
        }

        [Fact]
        public void GetDiabeteRisk_Patient_Over30_HeightTriggers_ShouldReturn_EarlyOnset()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 8);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 8);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(expectedRisk, woman35Risk);
            Assert.Equal(expectedRisk, man35Risk);
        }
        #endregion

        #region GetNbTriggers
        [Fact]
        public void GetNbTriggers_ZeroNotes_ShouldReturn_0()
        {
            var notes = new List<string>();
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 0;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_WithEmptyNote_1_Trigger_ShouldReturn_1()
        {
            var notes = new List<string> { "Première note", "Deuxième Note", "", SearchedTriggers[0] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_Trigger_ShouldReturn_1()
        {
            var notes = new List<string> { "Première note", "Deuxième Note", SearchedTriggers[0] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_Trigger_SeveralTimes_ShouldReturn_1()
        {
            var notes = new List<string> { SearchedTriggers[0], SearchedTriggers[0], SearchedTriggers[0] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_2_Triggers_ShouldReturn_2()
        {
            var notes = new List<string> { "Première note", SearchedTriggers[0], SearchedTriggers[1] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 2;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_TriggerUpperCase_ShouldReturn_1()
        {
            var notes = new List<string> { "Première note", SearchedTriggers[0].ToUpper() };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_TriggerSeveralTimesWithSeveralCases_ShouldReturn_1()
        {
            var notes = new List<string> { SearchedTriggers[0], SearchedTriggers[0].ToUpper(), SearchedTriggers[0].ToLower() };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_3_TriggerSeveralTimesWithSeveralCases_ShouldReturn_3()
        {
            var notes = new List<string>
            { 
                "Première Note",
                SearchedTriggers[0], SearchedTriggers[0].ToUpper(), SearchedTriggers[0].ToLower(),
                SearchedTriggers[1], SearchedTriggers[1].ToUpper(), SearchedTriggers[1].ToLower(),
                SearchedTriggers[2], SearchedTriggers[2].ToUpper(), SearchedTriggers[2].ToLower()
            };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 3;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_TestCase1_ShouldReturn_1()
        {
            var notes = new List<string>
            {
                "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé"
            };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_TestCase2_ShouldReturn_2()
        {
            var notes = new List<string>
            {
                "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement",
                "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale"
            };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 2;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_TestCase3_ShouldReturn_3()
        {
            var notes = new List<string>
            {
                "Le patient déclare qu'il fume depuis peu",
                "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé"
            };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 3;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }

        [Fact]
        public void GetNbTriggers_TestCase4_ShouldReturn_6()
        {
            var notes = new List<string>
            {
                "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments",
                "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps",
                "Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé",
                "Taille, Poids, Cholestérol, Vertige et Réaction"
            };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 6;

            Assert.Equal(expectedNbTriggers, nbTriggers);
        }
        #endregion
    }
}
