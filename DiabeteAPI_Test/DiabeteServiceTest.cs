using DiabeteAPI.Services;
using static DiabeteAPI.Services.DiabeteService;

namespace DiabeteAPI_Test
{
    public class DiabeteServiceTest
    {
        private readonly DiabeteService _diabeteService;

        public DiabeteServiceTest()
        {
            _diabeteService = new DiabeteService();
        }

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
    }
}
