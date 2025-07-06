using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Queries;

public record GetAppointmentsByStatusQuery(
    AppointmentStatus Status,
    DateTime? FromDate = null
) : IRequest<IEnumerable<Appointment>>;