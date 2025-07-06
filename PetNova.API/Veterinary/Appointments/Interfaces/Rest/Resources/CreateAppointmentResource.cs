using System.ComponentModel.DataAnnotations;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

public class CreateAppointmentResource
{
    [Required]
    public Guid PetId { get; set; }
    
    [Required]
    public Guid ClientId { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public TimeSpan Duration { get; set; }
    
    [Required]
    public string Type { get; set; }  // Se valida contra el enum
}