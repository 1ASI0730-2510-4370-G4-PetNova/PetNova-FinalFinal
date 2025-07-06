using System;
using System.ComponentModel.DataAnnotations;

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources
{
    public record CreateAppointmentResource(
        [Required] Guid PetId,
        [Required] Guid ClientId,
        [Required] DateTime StartDate,
        [Required] int Duration,
        [Required] string Type
    );
}
