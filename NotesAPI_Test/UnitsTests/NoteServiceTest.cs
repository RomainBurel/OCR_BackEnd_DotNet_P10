using Moq;
using NotesAPI.Domain;
using NotesAPI_SharedModels;
using NotesAPI.Repositories;
using NotesAPI.Services;

namespace NotesAPI_Test.UnitsTests
{
    public class NoteServiceTest
    {
        private readonly Mock<INoteRepository> _mockRepository;
        private readonly NoteService _noteService;

        public NoteServiceTest()
        {
            _mockRepository = new Mock<INoteRepository>();
            _noteService = new NoteService(_mockRepository.Object);
        }

        private IEnumerable<Note> GetNotes()
        {
            return new List<Note>
            {
                new()
                {
                    NoteId = "1",
                    PatientId = 1,
                    Content = "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "2",
                    PatientId = 2,
                    Content = "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "3",
                    PatientId = 2,
                    Content = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "4",
                    PatientId = 3,
                    Content = "Le patient déclare qu'il fume depuis peu",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "5",
                    PatientId = 3,
                    Content = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "6",
                    PatientId = 4,
                    Content = "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "7",
                    PatientId = 4,
                    Content = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "8",
                    PatientId = 4,
                    Content = " Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                },
                new()
                {
                    NoteId = "9",
                    PatientId = 4,
                    Content = "Taille, Poids, Cholestérol, Vertige et Réaction",
                    CreatedAt = new DateTime(2022, 09, 01),
                    UpdatedAt = new DateTime(2022, 09, 01)
                }
            };
        }

        private IEnumerable<Note> GetPatientNotes(int patientId)
        {
            return GetNotes().Where(note => note.PatientId == patientId);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfNoteModel()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllForAPatient(It.IsAny<int>())).Returns<int>(patientId => Task.Run(() => GetPatientNotes(patientId)));

            // Act
            var patient1Notes = await _noteService.GetAllForAPatient(1);
            var patient2Notes = await _noteService.GetAllForAPatient(2);
            var patient3Notes = await _noteService.GetAllForAPatient(3);
            var patient4Notes = await _noteService.GetAllForAPatient(4);

            // Assert
            Assert.Equal(1, patient1Notes.Count());
            Assert.Equal(2, patient2Notes.Count());
            Assert.Equal(2, patient3Notes.Count());
            Assert.Equal(4, patient4Notes.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnBidList_WhenExists()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById("1")).Returns(Task.Run(() => GetNotes().ToList()[0]));

            // Act
            var result = await _noteService.GetById("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.NoteId);
            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task Add_ShouldCall_RepositoryAdd()
        {
            // Arrange
            var newNoteModel = new NoteModelAdd
            {
                PatientId = 4,
                Content = "New note"
            };

            // Act
            await _noteService.Add(newNoteModel);

            // Assert
            _mockRepository.Verify(repo => repo.Add(It.Is<Note>(b => b.PatientId == 4 && b.Content == "New note")), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCall_RepositoryUpdate()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById("1")).Returns(Task.Run(() => GetNotes().ToList()[0]));
            _mockRepository.Setup(repo => repo.Update(It.IsAny<string>(), It.IsAny<Note>()));
            var patientUpdated = new NoteModelUpdate { Content = "Updated content" };

            // Act
            await _noteService.Update("1", patientUpdated);

            // Assert
            _mockRepository.Verify(repo => repo.Update(It.IsAny<string>(), It.Is<Note>(b => b.Content == "Updated content")), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCall_RepositoryRemove()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById("1")).Returns(Task.Run(() => GetNotes().ToList()[0]));
            _mockRepository.Setup(repo => repo.Delete(It.IsAny<string>()));

            // Act
            var patientModel = _noteService.GetById("1");
            await _noteService.Delete("1");

            // Assert
            _mockRepository.Verify(repo => repo.Delete(It.Is<string>(b => b == "1")), Times.Once);
        }
    }
}