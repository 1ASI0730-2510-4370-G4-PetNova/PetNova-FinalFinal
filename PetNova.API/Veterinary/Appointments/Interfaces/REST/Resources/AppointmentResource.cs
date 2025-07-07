namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;

public class AppointmentResource
{
    public Guid Id { get; set; }
    public string PetName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string Status { get; set; } = null!;
    public string Type { get; set; } = null!;
}
