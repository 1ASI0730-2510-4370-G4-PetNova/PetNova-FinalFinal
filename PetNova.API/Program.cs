using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using PetNova.API.Shared.Application.Services;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Repositories;
using PetNova.API.Shared.Infrastructure.Services;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.Appointments.Application.Services;
using PetNova.API.Veterinary.IAM.Application.Services;
using PetNova.API.Veterinary.MedicalHistory.Application.Services;
using PetNova.API.Veterinary.Status.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────
// 1️⃣  REGISTER SERVICES
// ───────────────────────────────────────────────

// Domain services (DI)
//builder.Services.AddScoped<PetService>();
//builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<IClientService, ClientService>();
//builder.Services.AddScoped<DoctorService>();
//builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<MedicalRecordService>();
//builder.Services.AddScoped<StatusService>();

// Generic repository & UoW
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JWT token service (scoped para evitar singleton‑state)
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "PetNova API",
        Version     = "v1",
        Description = "PetNova veterinary management API"
    });
});

// ───────────────────────────────────────────────
// 2️⃣  DATABASE CONTEXT
// ───────────────────────────────────────────────
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

    if (builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information);
});

// ───────────────────────────────────────────────
// 3️⃣  AUTHENTICATION & AUTHORIZATION
// ───────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer   = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT Key not found")))
        };
    });

builder.Services.AddAuthorization();

// ───────────────────────────────────────────────
// 4️⃣  BUILD APP
// ───────────────────────────────────────────────
var app = builder.Build();

// Global exception page in Dev
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Swagger always available
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS & routing
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ───────────────────────────────────────────────
// 5️⃣  MIGRATIONS & SEED
// ───────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();          // crea/actualiza
        await SeedData.InitializeAsync(services);       // datos iniciales
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error while migrating or seeding the database.");
    }
}

app.Run();