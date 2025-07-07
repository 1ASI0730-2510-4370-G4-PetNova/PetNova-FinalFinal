using System;
using System.Threading.Tasks;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;

namespace PetNova.API.Veterinary.Appointments.Domain.Services;
public interface IAppointmentCommandService
{
    Task<Appointment?> Handle(CreateAppointmentCommand command);
    Task<Appointment?> Handle(Guid id, CreateAppointmentCommand command);
    Task<bool> Handle(DeleteAppointmentCommand command);
}