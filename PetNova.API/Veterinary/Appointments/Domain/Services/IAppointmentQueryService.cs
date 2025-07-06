using PetNova.API.Veterinary.Appointments.Domain.Model.Queries;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Domain.Services;

public interface IAppointmentQueryService
{
    Task<IEnumerable<Appointment>> HandleAsync(GetAllAppointmentsQuery query);
    Task<Appointment> HandleAsync(GetAppointmentByIdQuery query);
    Task<IEnumerable<Appointment>> HandleAsync(GetAppointmentsByStatusQuery query);
}