using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Queries;

public record GetAppointmentByIdQuery : IRequest<Appointment>
{
    public Guid Id { get; }
    
    public GetAppointmentByIdQuery(Guid id)
    {
        Id = id;
    }
}