using MongoDB.Driver;
using NotesAPI.Domain;

namespace NotesAPI.Data
{
    public class SeedData
    {
        private readonly NoteDbContext _context;

        public SeedData(NoteDbContext context)
        {
            this._context = context;
        }

        public async Task Seed()
        {
            if (await _context.Notes.CountDocumentsAsync<Note>(n => n.PatientId > 0) == 0)
            {
                var notes = new List<Note>
                {
                    new()
                    {
                        PatientId = 1,
                        Content = "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 2,
                        Content = "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 2,
                        Content = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 3,
                        Content = "Le patient déclare qu'il fume depuis peu",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 3,
                        Content = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 4,
                        Content = "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 4,
                        Content = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 4,
                        Content = " Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    },
                    new()
                    {
                        PatientId = 4,
                        Content = "Taille, Poids, Cholestérol, Vertige et Réaction",
                        CreatedAt = new DateTime(2022, 09, 01),
                        UpdatedAt = new DateTime(2022, 09, 01)
                    }
                };

                await this._context.Notes.InsertManyAsync(notes);
            }
        }
    }
}
