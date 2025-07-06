using PetNova.API.Shared.Domain;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using System;

namespace PetNova.API.Veterinary.Appointments.Domain.Model;

public class Appointment : Entity<Guid>
{
    public Guid PetId { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime StartDate { get; private set; }
    public TimeSpan Duration { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public AppointmentType Type { get; private set; }

    private Appointment() {}

    public static Appointment Create(Guid petId, Guid clientId, DateTime startDate, TimeSpan duration, AppointmentType type)
    {
        

        return new Appointment
        {
            Id = Guid.NewGuid(),
            PetId = petId,
            ClientId = clientId,
            StartDate = startDate,
            Duration = duration,
            Status = AppointmentStatus.Pending,
            Type = type
        };
    }

    public void Reschedule(DateTime newDate)
    {
        if (newDate < DateTime.UtcNow.AddHours(24))
            throw new InvalidOperationException("Nueva fecha debe tener al menos 24h de anticipación.");

        StartDate = newDate;
        Status = AppointmentStatus.Rescheduled;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
            throw new InvalidOperationException("No puedes cancelar una cita completada.");

        Status = AppointmentStatus.Cancelled;
    }

    public void Complete() => Status = AppointmentStatus.Completed;

    public void ChangeStatus(AppointmentStatus newStatus) => Status = newStatus;
}