using PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Transform;

public static class AppointmentResourceAssembler
{
    public static AppointmentResource ToResource(Appointment appointment)
        => new()
    {
        Id = appointment.Id,
        PetName = appointment.PetName,
        ClientName = appointment.ClientName,
        ContactNumber = appointment.ContactNumber,
        StartDate = appointment.StartDate,
        Status = appointment.Status.ToString(),
        Type = appointment.Type.ToString()
    };
}
