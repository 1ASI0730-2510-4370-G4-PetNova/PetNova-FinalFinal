using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

namespace PetNova.API.Veterinary.Appointments.Domain.Services;

public interface IAppointmentCommandService
{
    Task<Appointment> HandleAsync(CreateAppointmentCommand command);
    Task HandleAsync(UpdateAppointmentCommand command);
    Task HandleAsync(DeleteAppointmentCommand command);
}