using Microsoft.Extensions.DependencyInjection;
using PetNova.API.Veterinary.Doctor.Configuration;

namespace PetNova.API.Veterinary.Doctor.Configuration;

public static class ProgramExtensions
{
    public static WebApplicationBuilder ConfigureDoctorServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDoctorApplication()
            .AddDoctorInfrastructure();
        
        return builder;
    }
}