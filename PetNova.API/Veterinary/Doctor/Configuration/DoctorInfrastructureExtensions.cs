using Microsoft.Extensions.DependencyInjection;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;
using PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC;
using PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC.Repositories;

namespace PetNova.API.Veterinary.Doctor.Configuration
{
    public static class DoctorInfrastructureExtensions
    {
        public static IServiceCollection AddDoctorInfrastructure(this IServiceCollection services)
        {
            // Registra aquí las implementaciones de repositorios y servicios de infraestructura
            services.AddScoped<IProfileRepository, ProfileRepository>();
            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            // Registra aquí los servicios de persistencia adicionales
            return services;
        }
    }
}