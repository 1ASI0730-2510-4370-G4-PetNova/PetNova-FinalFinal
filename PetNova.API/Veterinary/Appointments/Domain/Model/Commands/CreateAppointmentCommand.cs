using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

public record CreateAppointmentCommand(
    Guid PetId,
    string PetName,
    Guid ClientId,
    string ClientName,
    string ContactNumber,
    DateTime StartDate,
    AppointmentStatus Status,
    AppointmentType Type
);

