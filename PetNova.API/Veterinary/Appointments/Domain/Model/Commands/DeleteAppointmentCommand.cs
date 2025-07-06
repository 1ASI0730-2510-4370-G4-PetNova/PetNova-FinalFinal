using MediatR;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

public record DeleteAppointmentCommand : IRequest
{
    public Guid Id { get; }
    
    public DeleteAppointmentCommand(Guid id)
    {
        Id = id;
    }
}