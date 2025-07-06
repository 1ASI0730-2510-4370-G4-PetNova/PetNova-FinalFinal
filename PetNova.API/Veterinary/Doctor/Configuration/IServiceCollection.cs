using Microsoft.Extensions.DependencyInjection;
using PetNova.API.Veterinary.Doctor.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Doctor.Domain.Model.Queries;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;

namespace PetNova.API.Veterinary.Doctor.Configuration;

public static class DoctorApplicationExtensions
{
    public static IServiceCollection AddDoctorApplication(this IServiceCollection services)
    {
        services.AddScoped<IProfileQueryService, ProfileQueryService>();
        // Agrega aquí otros servicios de la capa de aplicación
        return services;
    }
}