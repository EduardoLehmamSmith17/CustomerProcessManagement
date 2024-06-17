using CustomerProcessManagement.Data;
using CustomerProcessManagement.Repositories;
using CustomerProcessManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura��o da string de conex�o do PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SystemProcessManagementcsDBContext>(options =>
    options.UseNpgsql(connectionString));

// Adiciona seus reposit�rios e interfaces aqui
builder.Services.AddScoped<IPhysicalPersonRepositorie, PhysicalPersonRepositorie>();
builder.Services.AddScoped<ILegalPersonRepositorie, LegalPersonRepositorie>();

// Adiciona outros servi�os necess�rios
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();

// Executa a aplica��o
app.Run();