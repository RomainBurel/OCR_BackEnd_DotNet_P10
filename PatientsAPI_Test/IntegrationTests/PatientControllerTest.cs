using PatientsAPI.Domain;
using PatientsAPI_SharedModels;
using System.Net;
using System.Net.Http.Json;

namespace PatientsAPI_Test.IntegrationTests
{
    [Collection("IntegrationTests")]
    public class PatientControllerTest : GenericControllerTest<Patient>
    {
        public PatientControllerTest(CustomPatientsAPIFactory<PatientsAPI.Program> factoryPatient) : base(factoryPatient)
        {
        }

        #region Data initialization
        private List<Patient> GetPatients()
        {
            return new List<Patient> {
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

        private PatientModelAdd GetNewPatientModelAdd()
        {
            return new PatientModelAdd()
            { 
                LastName = "TesPatientAdd",
                FirstName = "TestPatientAdd",
                DateOfBirth = new DateTime(1992, 12, 12),
                GenderId = 1
            };
        }

        private PatientModelUpdate GetPatientModelToUpdate(Patient patient)
        {
            return new PatientModelUpdate()
            {
                LastName = "TesPatientUpdate",
                FirstName = "TesPatientUpdate",
                DateOfBirth = new DateTime(1992, 12, 12),
                GenderId = 1
            };
        }

        private async Task SeedSamplePatientsAsync()
        {
            await ClearTable();
            await FillTable(GetPatients());
        }
        #endregion

        [Fact]
        public async Task GetAll_ShouldReturn_AllRecords()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var nbRecords = NbRecordsInTable();

            // Act
            var response = await _httpClientPatient.GetAsync("/Patient/list");
            var patients = await response.Content.ReadFromJsonAsync<List<PatientModel>>();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(patients);
            Assert.Equal(nbRecords, patients.Count);
        }

        [Fact]
        public async Task GetPatientById_ShouldReturn_Record()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var expectedPatient = GetFirstRecordInTable();

            // Act
            var response = await _httpClientPatient.GetAsync($"/Patient/display/{expectedPatient.PatientId}");
            var patient = await response.Content.ReadFromJsonAsync<PatientModel>();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(patient);
            Assert.Equal(expectedPatient.PatientId, patient.PatientId);
            Assert.Equal(expectedPatient.LastName, patient.LastName);
            Assert.Equal(expectedPatient.FirstName, patient.FirstName);
        }

        [Fact]
        public async Task AddPatient_ShouldIncrease_NbRecords()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var nbRecordsInit = NbRecordsInTable();
            var newPatient = GetNewPatientModelAdd();

            // Act
            var response = await _httpClientPatient.PostAsJsonAsync("/Patient/creation", newPatient);
            var nbRecordsAfterAdd = NbRecordsInTable();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(nbRecordsAfterAdd, nbRecordsInit + 1);
        }

        [Fact]
        public async Task UpdatePatient_ShouldReturn_Ok()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var patientToUpdate = GetFirstRecordInTable();
            var patientModelUpdate = GetPatientModelToUpdate(patientToUpdate);
            patientModelUpdate.FirstName = "NewFirstName";

            // Act
            var response = await _httpClientPatient.PutAsJsonAsync($"/Patient/update/{patientToUpdate.PatientId}", patientModelUpdate);
            var patientUpdated = GetRecordById(patientToUpdate.PatientId);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(patientUpdated.FirstName, patientModelUpdate.FirstName);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePatient_ForNonExistingId_ShouldReturn_NotFound()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var responseAll = await _httpClientPatient.GetAsync("/Patient/list");
            var patientToUpdate = GetFirstRecordInTable();
            var patientModelUpdate = GetPatientModelToUpdate(patientToUpdate);
            patientModelUpdate.FirstName = "NewFirstName";
            var nonExistingPatientId = -1;

            // Act
            var response = await _httpClientPatient.PutAsJsonAsync($"/Patient/update/{nonExistingPatientId}", patientModelUpdate);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletePatient_ShouldReturn_Ok()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var patientToDeleteId = GetFirstRecordInTable().PatientId;

            // Act
            var response = await _httpClientPatient.DeleteAsync($"/Patient/deletion/{patientToDeleteId}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeletePatient_ForNonExistingId_ShouldReturn_NotFound()
        {
            // Arrange
            await SeedSamplePatientsAsync();
            var nonExistingPatientId = -1;

            // Act
            var response = await _httpClientPatient.DeleteAsync($"/Patient/deletion/{nonExistingPatientId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
