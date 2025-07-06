namespace PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC.Repositories;
using PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Configuración de DbContext para MySQL
        services.AddDbContext<DoctorDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                mysqlOptions =>
                {
                    mysqlOptions.MigrationsAssembly(typeof(DoctorDbContext).Assembly.FullName);
                    mysqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
        });

        // Configuración de Repositorios
        services.AddScoped<IProfileRepository, ProfileRepository>();

        return services;
    }
}