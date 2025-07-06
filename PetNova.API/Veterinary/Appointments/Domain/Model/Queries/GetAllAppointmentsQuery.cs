using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Queries;

public record GetAllAppointmentsQuery(int Page = 1, int PageSize = 20);
