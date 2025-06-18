var builder = WebApplication.CreateBuilder(args);

// Habilita controladores y Swagger
builder.Services.AddControllers(); // <-- Esto es CLAVE
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Puerto para Render o local
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

//Swagger UI (activado siempre para asegurar visibilidad)
app.UseSwagger();
app.UseSwaggerUI();

// Evita redirección HTTPS para evitar errores locales
// app.UseHttpsRedirection(); // Desactivado temporalmente

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // <-- Esto permite que funcione /api/test

app.Run();
