using System.ComponentModel.DataAnnotations;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

public class UpdateAppointmentResource
{
    [Required]
    public Guid Id { get; set; }
    
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }  // Nullable para actualizaciones parciales
}