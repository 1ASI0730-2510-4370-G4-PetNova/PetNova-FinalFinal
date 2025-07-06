using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects; // For AppointmentType
using System; // For TimeSpan

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Transform
{
    public static class CreateAppointmentCommandFromResourceAssembler
    {
        public static CreateAppointmentCommand ToCommandFromResource(CreateAppointmentResource resource)
        {
            if (!Enum.TryParse<AppointmentType>(resource.Type, true, out var appointmentType))
            {
                // Handle invalid type string, e.g., throw an exception or default
                throw new ArgumentException($"Invalid appointment type: {resource.Type}");
            }

            return new CreateAppointmentCommand(
                resource.PetId,
                resource.ClientId,
                resource.StartDate,
                TimeSpan.FromMinutes(resource.Duration), // Convert int minutes to TimeSpan
                appointmentType // Use parsed enum
            );
        }
    }
}
