using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Cargar ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile(
    $"ocelot.{builder.Environment.EnvironmentName}.json",
    optional: true,
    reloadOnChange: true);

// Registrar Ocelot
builder.Services.AddOcelot(builder.Configuration);

// CORS para el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseAuthentication();
await app.UseOcelot();

app.Run();