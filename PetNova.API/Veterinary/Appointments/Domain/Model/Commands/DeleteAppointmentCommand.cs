namespace PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

/// <summary>
/// Command to delete an appointment.
/// </summary>
/// <param name="AppointmentId">ID of the appointment to delete.</param>
public record DeleteAppointmentCommand(Guid AppointmentId);