using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

public record CreateAppointmentCommand(
    Guid PetId,
    Guid ClientId,
    DateTime StartDate,
    TimeSpan Duration,
    AppointmentType Type
) : IRequest<Appointment>;