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
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
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