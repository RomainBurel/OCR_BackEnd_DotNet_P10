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
            var httpMock = new Mock<HttpClient>();
            _diabeteService = new DiabeteService(httpMock.Object);
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
        public void Patient_LessThanTwoTriggers_HasRiskNone()
        {
            var woman25Risk = _diabeteService.GetDiabeteRisk(2, 25, 1);
            var woman30Risk = _diabeteService.GetDiabeteRisk(2, 25, 0);
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 1);
            var man25Risk = _diabeteService.GetDiabeteRisk(1, 25, 1);
            var man30Risk = _diabeteService.GetDiabeteRisk(1, 25, 0);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 1);
            var expectedRisk = DiabeteRisk.None;

            Assert.Equal(woman25Risk, expectedRisk);
            Assert.Equal(woman30Risk, expectedRisk);
            Assert.Equal(woman35Risk, expectedRisk);
            Assert.Equal(man25Risk, expectedRisk);
            Assert.Equal(man30Risk, expectedRisk);
            Assert.Equal(man35Risk, expectedRisk);
        }

        [Fact]
        public void Patient_Over30_FiveTriggers_HasRiskBorderline()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 5);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 5);
            var expectedRisk = DiabeteRisk.BorderLine;

            Assert.Equal(woman35Risk, expectedRisk);
            Assert.Equal(man35Risk, expectedRisk);
        }

        [Fact]
        public void Man_Under30_ThreeTriggers_HasRiskInDanger()
        {
            var man25Risk = _diabeteService.GetDiabeteRisk(1, 25, 3);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(man25Risk, expectedRisk);
        }

        [Fact]
        public void Woman_Under30_FourTriggers_HasRiskInDanger()
        {
            var woman25Risk = _diabeteService.GetDiabeteRisk(2, 25, 4);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(woman25Risk, expectedRisk);
        }

        [Fact]
        public void Patient_Over30_SixTriggers_HasRiskInDanger()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 6);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 6);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(woman35Risk, expectedRisk);
            Assert.Equal(man35Risk, expectedRisk);
        }

        [Fact]
        public void Patient_Over30_SevenTriggers_HasRiskInDanger()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 7);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 7);
            var expectedRisk = DiabeteRisk.InDanger;

            Assert.Equal(woman35Risk, expectedRisk);
            Assert.Equal(man35Risk, expectedRisk);
        }

        [Fact]
        public void Man_Under30_FiveTriggers_HasRiskEarlyOnset()
        {
            var man25Risk = _diabeteService.GetDiabeteRisk(1, 25, 5);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(man25Risk, expectedRisk);
        }

        [Fact]
        public void Woman_Under30_SevenTriggers_HasRiskEarlyOnset()
        {
            var woman25Risk = _diabeteService.GetDiabeteRisk(2, 25, 7);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(woman25Risk, expectedRisk);
        }

        [Fact]
        public void Patient_Over30_HeightTriggers_HasRiskEarlyOnset()
        {
            var woman35Risk = _diabeteService.GetDiabeteRisk(2, 35, 8);
            var man35Risk = _diabeteService.GetDiabeteRisk(1, 35, 8);
            var expectedRisk = DiabeteRisk.EarlyOnset;

            Assert.Equal(woman35Risk, expectedRisk);
            Assert.Equal(man35Risk, expectedRisk);
        }
        #endregion

        #region GetNbTriggers
        [Fact]
        public void GetNbTriggers_ZeroNotes_Returns_0()
        {
            var notes = new List<string>();
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 0;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_WithEmptyNote_1_Trigger_Returns_1()
        {
            var notes = new List<string> { "Première note", "Deuxième Note", "", SearchedTriggers[0] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_Trigger_Returns_1()
        {
            var notes = new List<string> { "Première note", "Deuxième Note", SearchedTriggers[0] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_Trigger_SeveralTimes_Returns_1()
        {
            var notes = new List<string> { SearchedTriggers[0], SearchedTriggers[0], SearchedTriggers[0] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_2_Triggers_Returns_2()
        {
            var notes = new List<string> { "Première note", SearchedTriggers[0], SearchedTriggers[1] };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 2;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_TriggerUpperCase_Returns_1()
        {
            var notes = new List<string> { "Première note", SearchedTriggers[0].ToUpper() };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_1_TriggerSeveralTimesWithSeveralCases_Returns_1()
        {
            var notes = new List<string> { SearchedTriggers[0], SearchedTriggers[0].ToUpper(), SearchedTriggers[0].ToLower() };
            int nbTriggers = _diabeteService.GetNbTriggers(notes);
            int expectedNbTriggers = 1;

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }

        [Fact]
        public void GetNbTriggers_3_TriggerSeveralTimesWithSeveralCases_Returns_3()
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

            Assert.Equal(nbTriggers, expectedNbTriggers);
        }
        #endregion
    }
}
