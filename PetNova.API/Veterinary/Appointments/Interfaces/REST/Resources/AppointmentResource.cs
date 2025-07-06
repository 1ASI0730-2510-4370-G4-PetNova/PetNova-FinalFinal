using System;

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources
{
    public record AppointmentResource(
        Guid Id,
        Guid PetId,
        Guid ClientId,
        DateTime StartDate,
        int Duration,
        string Type,
        string Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
