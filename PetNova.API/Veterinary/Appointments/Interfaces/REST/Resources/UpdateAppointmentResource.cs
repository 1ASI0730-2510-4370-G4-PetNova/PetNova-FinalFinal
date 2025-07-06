using System;

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources
{
    public record UpdateAppointmentResource(
        DateTime? NewStartDate,
        string? NewStatus
    );
}
