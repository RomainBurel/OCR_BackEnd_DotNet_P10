using Moq;
using PatientsAPI.Domain;
using PatientsAPI.Models;
using PatientsAPI.Repositories;
using PatientsAPI.Services;
using System.Runtime.InteropServices;

namespace PatientsAPI_Test.UnitsTests
{
    public class PatientServiceTest
    {
        private readonly Mock<IPatientRepository> _mockRepository;
        private readonly PatientService _patientService;

        public PatientServiceTest()
        {
            _mockRepository = new Mock<IPatientRepository>();
            _patientService = new PatientService(_mockRepository.Object);
        }

        private IEnumerable<Patient> GetPatients()
        {
            return new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FirstName = "Test",
                    LastName = "TestNone",
                    DateOfBirth = new DateTime(1966, 12, 31),
                    GenderId = 2,
                    Address = "1 Brookside St",
                    PhoneNumber = "100-222-3333"
                },
                new Patient
                {
                    PatientId = 2,
                    FirstName = "Test",
                    LastName = "TestBorderline",
                    DateOfBirth = new DateTime(1945, 06, 24),
                    GenderId = 1,
                    Address = "2 High St",
                    PhoneNumber = "200-333-4444"
                },
                new Patient
                {
                    PatientId = 3,
                    LastName = "TestDanger",
                    FirstName = "Test",
                    DateOfBirth = new DateTime(2004, 06, 18),
                    GenderId = 1,
                    Address = "3 Club Road",
                    PhoneNumber = "300-444-5555"
                },
                new Patient
                {
                    PatientId = 4,
                    FirstName = "Test",
                    LastName = "TestEarlyOnset",
                    DateOfBirth = new DateTime(2002, 06, 28),
                    GenderId = 2,
                    Address = "4 Valley Dr",
                    PhoneNumber = "400-555-6666"
                }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfBidListModel()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll()).Returns(Task.Run(() => GetPatients()));

            // Act
            var result = await _patientService.GetAll();

            // Assert
            Assert.Equal(4, result.Count());
            Assert.Contains(result, b => b.PatientId == 1 && b.LastName == "TestNone");
            Assert.Contains(result, b => b.PatientId == 2 && b.LastName == "TestBorderline");
            Assert.Contains(result, b => b.PatientId == 3 && b.LastName == "TestDanger");
            Assert.Contains(result, b => b.PatientId == 4 && b.LastName == "TestEarlyOnset");
        }

        [Fact]
        public async Task GetById_ShouldReturnBidList_WhenExists()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(Task.Run(() => GetPatients().ToList()[0]));

            // Act
            var result = await _patientService.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
            Assert.Equal("TestNone", result.LastName);
        }

        [Fact]
        public async Task Add_ShouldCall_RepositoryAdd()
        {
            // Arrange
            var newPatientModel = new PatientModelAdd
            {
                FirstName = "TestNewA",
                LastName = "TestNewB",
                DateOfBirth = new DateTime(2000, 01, 01),
                GenderId = 2,
                Address = "5 City Th",
                PhoneNumber = "123-456-7890"
            };

            // Act
            await _patientService.Add(newPatientModel);

            // Assert
            _mockRepository.Verify(repo => repo.Add(It.Is<Patient>(b => b.FirstName == "TestNewA" && b.LastName == "TestNewB")), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCall_RepositoryUpdate()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(Task.Run(() => GetPatients().ToList()[0]));
            _mockRepository.Setup(repo => repo.Update(It.IsAny<Patient>()));
            var patientUpdated = new PatientModelUpdate { FirstName = "TestUpdatedA", LastName = "TestUpdatedB" };

            // Act
            await _patientService.Update(1, patientUpdated);

            // Assert
            _mockRepository.Verify(repo => repo.Update(It.Is<Patient>(b => b.FirstName == "TestUpdatedA" && b.LastName == "TestUpdatedB")), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCall_RepositoryRemove()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(Task.Run(() => GetPatients().ToList()[0]));
            _mockRepository.Setup(repo => repo.Delete(It.IsAny<int>()));

            // Act
            var patientModel = _patientService.GetById(1);
            await _patientService.Delete(1);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(It.Is<int>(b => b == 1)), Times.Once);
        }
    }
}