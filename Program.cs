using Microsoft.EntityFrameworkCore;
using MundialClubesApi.Data;
using MundialClubesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura EF Core
builder.Services.AddDbContext<FutbolDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
});

// Cliente HTTP para API-Football
builder.Services.AddHttpClient<ApiFootballService>(client =>
{
    client.BaseAddress = new Uri("https://v3.football.api-sports.io/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "ac0eef5a16b0719f79b4d20ba1a2cf17");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "v3.football.api-sports.io");
});

builder.Services.AddHttpClient<TheSportsDbService>();
builder.Services.AddHttpClient<TransferenciasService>();
builder.Services.AddHttpClient<JugadoresService>();


// Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 💡 Habilita CORS aquí
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Puerto dinámico o fijo
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

// 💡 Usa CORS aquí
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
