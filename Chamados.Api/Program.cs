using Chamados.Core.DataAccess;
using Chamados.Core.ViewModels;
using Chamados.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar a camada de dados e ViewModels do Chamados.Core
builder.Services.AddChamadosCoreServices(builder.Configuration.GetConnectionString("PostgreSQLConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
