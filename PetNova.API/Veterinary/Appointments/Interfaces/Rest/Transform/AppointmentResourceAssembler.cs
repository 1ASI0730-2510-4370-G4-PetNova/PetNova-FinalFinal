using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest.Transform;

public static class AppointmentResourceAssembler
{
    public static AppointmentResource ToResource(Appointment appointment) => new()
    {
        Id = appointment.Id,
        PetId = appointment.PetId,
        ClientId = appointment.ClientId,
        StartDate = appointment.StartDate,
        Duration = appointment.Duration,
        Status = appointment.Status.ToString(),
        Type = appointment.Type.ToString()
    };
}