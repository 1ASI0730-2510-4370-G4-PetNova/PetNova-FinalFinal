namespace PetNova.API.Veterinary.Appointments.Interfaces.DTOs;

public class AppointmentDto
{
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string Notes { get; set; }
    public Guid PetId { get; set; }
    public Guid DoctorId { get; set; }
}