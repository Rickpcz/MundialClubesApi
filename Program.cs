using Microsoft.EntityFrameworkCore;
using MundialClubesApi.Data;
using MundialClubesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura EF Core con MySQL (usa la cadena de conexión desde appsettings.json)
builder.Services.AddDbContext<FutbolDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))); // Ajusta versión según tu host
});

// 2. Inyecta el servicio HTTP para consumir API-Football
builder.Services.AddHttpClient<ApiFootballService>(client =>
{
    client.BaseAddress = new Uri("https://v3.football.api-sports.io/");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "ac0eef5a16b0719f79b4d20ba1a2cf17");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "v3.football.api-sports.io");
});

// 3. Habilita controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Puerto dinámico para Render o 5000 localmente
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

// 5. Middleware de desarrollo y Swagger siempre activo
app.UseSwagger();
app.UseSwaggerUI();

// 6. HTTPS redirection desactivado para evitar errores locales
// app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
