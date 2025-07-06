using MediatR;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

public record DeleteAppointmentCommand(Guid Id);
