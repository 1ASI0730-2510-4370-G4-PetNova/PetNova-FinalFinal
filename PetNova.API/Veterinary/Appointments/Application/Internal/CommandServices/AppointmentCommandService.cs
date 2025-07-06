using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Appointments.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Veterinary.Appointments.Domain.Services;

namespace PetNova.API.Veterinary.Appointments.Application.Internal.CommandServices;

public class AppointmentCommandService : IAppointmentCommandService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentCommandService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Appointment> HandleAsync(CreateAppointmentCommand command)
    {
        var appointment = Appointment.Create(command.PetId, command.ClientId, command.StartDate, command.Duration, command.Type);
        await _repository.AddAsync(appointment);
        return appointment;
    }

    public async Task HandleAsync(UpdateAppointmentCommand command)
    {
        var appointment = await _repository.GetByIdAsync(command.Id) ?? throw new KeyNotFoundException();

        if (command.NewStartDate.HasValue)
            appointment.Reschedule(command.NewStartDate.Value);

        if (command.NewStatus.HasValue)
            appointment.ChangeStatus(command.NewStatus.Value); 

        await _repository.UpdateAsync(appointment);
    }

    public async Task HandleAsync(DeleteAppointmentCommand command)
    {
        await _repository.DeleteAsync(command.Id);
    }
}