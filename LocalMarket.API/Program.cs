using DotNetEnv;
using LocalMarket.API.Middleware;
using LocalMarket.Infrastructure;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

// .env configuration
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// JWT
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? throw new InvalidOperationException("JWT_SECRET_KEY no configurado");


var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
    ?? throw new InvalidOperationException("JWT_ISSUER no configurado");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            var problem = new ValidationProblemDetails(errors)
            {
                Type = "https://localmarket.com/errors/validation",
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path
            };

            return new BadRequestObjectResult(problem);
        };
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero // Sin margen extra de expiracion
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddInfrastructure();

//pipeline
// CORS para permitir conexión desde MAUI
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalMarketPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Aplicar migraciones automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
     await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("LocalMarketPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

