using System;
using System.Threading.Tasks;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Services;

namespace PetNova.API.Veterinary.Appointments.Application.Internal.CommandServices;

public class AppointmentCommandService(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
) : IAppointmentCommandService
{
    public async Task<Appointment?> Handle(CreateAppointmentCommand command)
    {
        var appointment = new Appointment(command);
        try
        {
            await appointmentRepository.AddAsync(appointment);
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return null;
        }

        return appointment;
    }

    public async Task<Appointment?> Handle(Guid id, CreateAppointmentCommand command)
    {
        var appointment = await appointmentRepository.GetByIdAsync(id);
        if (appointment is null) return null;

        appointment.Update(
            command.PetName,
            command.ClientId,
            command.ClientName,
            command.ContactNumber,
            command.StartDate,
            command.Status,
            command.Type
        );

        try
        {
            appointmentRepository.Update(appointment);
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return null;
        }

        return appointment;
    }

    public async Task<bool> Handle(DeleteAppointmentCommand command)
    {
        var appointment = await appointmentRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null) return false;

        try
        {
            appointmentRepository.Remove(appointment);
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return false;
        }

        return true;
    }
}