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
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Veterinary.Appointments.Application.Internal.CommandServices;
using PetNova.API.Veterinary.Appointments.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Appointments.Domain.Services;
using PetNova.API.Veterinary.Appointments.Infrastructure;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Transform;
using PetNova.API.Veterinary.Appointments.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// ───────────────────────────────────────────────
// 1️⃣ REGISTER SERVICES
// ───────────────────────────────────────────────

// Shared Services
builder.Services.AddScoped<IRepository<User, Guid>, EfRepository<User, Guid>>();
builder.Services.AddScoped<IRepository<Pet, Guid>, EfRepository<Pet, Guid>>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Pets Module
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetDomainCommandService, PetCommandService>();
builder.Services.AddScoped<IPetDomainQueryService, PetQueryService>();

// Doctor Module
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

// Appointments Module
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentCommandService, AppointmentCommandService>();
builder.Services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
builder.Services.AddAutoMapper(typeof(AppointmentMapper).Assembly);

// Generic repository & UoW
builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(EfRepository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerGen(options => 
{
    options.UseAllOfToExtendReferenceSchemas();
    options.UseAllOfForInheritance();
    options.UseOneOfForPolymorphism();
});

// ───────────────────────────────────────────────
// 2️⃣ DATABASE CONTEXTS
// ───────────────────────────────────────────────
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
              ?? throw new InvalidOperationException("Connection string not found");

// Registra AppDbContext para las entidades generales (como Pets y Clients)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

    if (builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information);
});

// Registra AppointmentsDbContext para las citas
builder.Services.AddDbContext<AppointmentsDbContext>(options =>
{
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

    if (builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information);
});

// Registra DoctorDbContext para los datos de los doctores
builder.Services.AddDbContext<DoctorDbContext>(options =>
{
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

    if (builder.Environment.IsDevelopment())
        options.LogTo(Console.WriteLine, LogLevel.Information);
});

// ───────────────────────────────────────────────
// 3️⃣ AUTHENTICATION & AUTHORIZATION
// ───────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT Key not found"))),
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCustomSwagger();

// ───────────────────────────────────────────────
// 4️⃣ BUILD APP
// ───────────────────────────────────────────────
var app = builder.Build();

// CORS
app.UseCors(MyAllowSpecificOrigins);

// Development Settings
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCustomSwaggerUI();

// ───────────────────────────────────────────────
// 5️⃣ MIGRATIONS & SEED
// ───────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var appointmentsContext = services.GetRequiredService<AppointmentsDbContext>();
        var doctorContext = services.GetRequiredService<DoctorDbContext>();

        await context.Database.MigrateAsync();
        await appointmentsContext.Database.MigrateAsync();
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
