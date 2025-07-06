using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

public record UpdateAppointmentCommand(Guid Id, DateTime? NewStartDate, AppointmentStatus? NewStatus);
