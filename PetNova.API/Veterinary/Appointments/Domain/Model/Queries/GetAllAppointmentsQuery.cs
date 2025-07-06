using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Queries;

public record GetAllAppointmentsQuery(
    int Page = 1,
    int PageSize = 20,
    string? SortBy = null
) : IRequest<IEnumerable<Appointment>>;