using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;
using PetNova.API.Veterinary.Appointments.Domain.Model.Query;

namespace PetNova.API.Veterinary.Appointments.Domain.Services;

public interface IAppointmentQueryService
{
    Task<IEnumerable<Appointment>> HandleAsync(GetAllAppointmentsQuery query);
    Task<Appointment?> HandleAsync(GetAppointmentByIdQuery query);

}