using System.Text;
using DepreciationService.Application.Common.Interfaces;
using DepreciationService.Application.UseCases.CalcularDepreciacion;
using DepreciationService.Domain.Services;
using DepreciationService.Infrastructure.Clients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Servicio de cálculo (domain service, sin estado)
builder.Services.AddSingleton<DepreciacionCalculator>();

// Cliente HTTP hacia AssetService
builder.Services.AddHttpClient<AssetServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Services:AssetService:BaseUrl"]!);
});
builder.Services.AddScoped<IAssetServiceClient>(services =>
    services.GetRequiredService<AssetServiceClient>());
builder.Services.AddScoped<CalcularDepreciacionHandler>();

// JWT Authentication (mismo esquema que AuthService)
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
