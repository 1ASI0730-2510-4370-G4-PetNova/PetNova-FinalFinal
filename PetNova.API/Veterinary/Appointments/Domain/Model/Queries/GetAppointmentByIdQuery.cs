using MediatR;
using PetNova.API.Veterinary.Appointments.Domain.Model;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Queries;

public record GetAppointmentByIdQuery(Guid Id);
