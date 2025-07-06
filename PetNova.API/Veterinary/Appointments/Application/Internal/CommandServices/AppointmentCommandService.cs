using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Appointments.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Veterinary.Appointments.Domain.Services;

namespace PetNova.API.Veterinary.Appointments.Application.Internal.CommandServices;

internal sealed class AppointmentCommandService : IAppointmentCommandService
{
    private readonly IAppointmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentCommandService(
        IAppointmentRepository repository, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Appointment> HandleAsync(CreateAppointmentCommand command)
    {
        var existingAppointment = await _repository.GetByDateTimeAsync(command.StartDate);
        if (existingAppointment != null)
            throw new DomainException("Ya existe una cita programada para este horario.");

        var appointment = Appointment.Create(
            command.PetId,
            command.ClientId,
            command.StartDate,
            command.Duration,
            command.Type
        );

        await _repository.AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
        return appointment;
    }

    public async Task HandleAsync(UpdateAppointmentCommand command)
    {
        var appointment = await _repository.GetByIdAsync(command.Id) 
            ?? throw new AppointmentNotFoundException(command.Id);

        if (command.NewStartDate.HasValue)
            appointment.Reschedule(command.NewStartDate.Value);

        if (command.NewStatus.HasValue)
            appointment.ChangeStatus(command.NewStatus.Value);

        await _repository.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task HandleAsync(DeleteAppointmentCommand command)
    {
        var appointment = await _repository.GetByIdAsync(command.Id) 
            ?? throw new AppointmentNotFoundException(command.Id);

        appointment.Cancel();
        await _repository.DeleteAsync(command.Id); // Cambiado a Guid
        await _unitOfWork.SaveChangesAsync();
    }
}

internal class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}