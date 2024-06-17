using CustomerProcessManagement.Data;
using CustomerProcessManagement.Repositories;
using CustomerProcessManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão do PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SystemProcessManagementcsDBContext>(options =>
    options.UseNpgsql(connectionString));

// Adiciona seus repositórios e interfaces aqui
builder.Services.AddScoped<IPhysicalPersonRepositorie, PhysicalPersonRepositorie>();
builder.Services.AddScoped<ILegalPersonRepositorie, LegalPersonRepositorie>();

// Adiciona outros serviços necessários
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

// Executa a aplicação
app.Run();