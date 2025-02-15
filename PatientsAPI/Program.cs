using Microsoft.EntityFrameworkCore;
using PatientsAPI.Data;
using PatientsAPI.Repositories;
using PatientsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajout des contrôleurs et configuration Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

var app = builder.Build();

// Activation de Swagger uniquement en mode développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
