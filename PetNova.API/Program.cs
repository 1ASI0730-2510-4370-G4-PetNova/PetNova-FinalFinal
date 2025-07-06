using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;   
using PetNova.API.Shared.Application.Services;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Repositories;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using PetNova.API.Veterinary.Doctor.Application.Internal.CommandServices;
using PetNova.API.Veterinary.Doctor.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Doctor.Domain.Model.Queries;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;
using PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC;
using PetNova.API.Veterinary.Doctor.Infrastructure.Service;
using PetNova.API.Veterinary.IAM.Application.Services;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Application.Internal.CommandServices;
using PetNova.API.Veterinary.Pets.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Repositories;
using PetNova.API.Veterinary.Pets.Domain.Services;
using PetNova.API.Veterinary.Pets.Infrastructure.Repositories;
using JwtTokenService = PetNova.API.Shared.Infrastructure.Services.JwtTokenService;


var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // 🔁 Aquí va la URL de tu frontend
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
// ───────────────────────────────────────────────
// 1️⃣  REGISTER SERVICES
// ───────────────────────────────────────────────

// Domain services (DI)
//builder.Services.AddScoped<IPetService   , PetService>();
builder.Services.AddScoped<IRepository<User, Guid>, EfRepository<User, Guid>>();
builder.Services.AddScoped<IRepository<Pet, Guid>, EfRepository<Pet, Guid>>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetDomainCommandService, PetCommandService>();
builder.Services.AddScoped<IPetDomainQueryService, PetQueryService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<AuthService>();

// Agregar estas líneas en la sección de registro de servicios
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();


// Generic repository & UoW
builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(EfRepository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JWT token service (scoped para evitar singleton‑state)
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options => 
{
    options.UseAllOfToExtendReferenceSchemas();
    options.UseAllOfForInheritance();
    options.UseOneOfForPolymorphism();
});
// Swagger
//builder.Services.AddEndpointsApiExplorer();
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

// Agregar junto con las demás configuraciones de servicios
builder.Services.AddDbContext<DoctorDbContext>(options =>
{
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

    if (builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information);
});

// Registrar los servicios necesarios
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

// Agregar junto con los demás servicios
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();

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
builder.Services.AddCustomSwagger();

// ───────────────────────────────────────────────
// 4️⃣  BUILD APP
// ───────────────────────────────────────────────
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
}

app.UseCors(MyAllowSpecificOrigins);

// Global exception page in Dev
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Swagger always available
// app.UseSwagger();
// app.UseSwaggerUI();

// HTTPS & routing
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCustomSwaggerUI();

// ───────────────────────────────────────────────
// 5️⃣  MIGRATIONS & SEED
// ───────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var doctorContext = services.GetRequiredService<DoctorDbContext>();
        
        await context.Database.MigrateAsync();
        await doctorContext.Database.MigrateAsync();
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            await doctorContext.Database.ExecuteSqlRawAsync("SELECT 1 FROM Profiles LIMIT 1");
            logger.LogInformation("Tabla 'Profiles' verificada correctamente");
        }
        catch
        {
            logger.LogWarning("La tabla 'Profiles' no existe, recreando esquema...");
            await doctorContext.Database.EnsureDeletedAsync();
            await doctorContext.Database.EnsureCreatedAsync();
        }
        
        await SeedData.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error durante la migración o inicialización de la base de datos.");
    }
}

app.Run();