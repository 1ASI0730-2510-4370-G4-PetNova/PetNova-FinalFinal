namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

public class AppointmentResource
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan Duration { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
}
