using Microsoft.EntityFrameworkCore;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Data Base Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Verify Data Base Connection String 
if (connectionString is null)
    throw new Exception("Connection string not found");
//Configure Data Base Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDBContext>(options =>
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
else if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDBContext>(options =>
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });
    
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.Run();
