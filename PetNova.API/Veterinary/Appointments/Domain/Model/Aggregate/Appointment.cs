using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Shared.Domain;
using PetNova.API.Veterinary.Appointments.Application.Internal.CommandServices;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;

public class Appointment : Entity<Guid>, IAggregateRoot
{
    public Guid PetId { get; private set; }
    public Guid ClientId { get; private set; }
    public DateTime StartDate { get; private set; }
    public TimeSpan Duration { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public AppointmentType Type { get; private set; }
    public object Pet { get; set; }
    public object Client { get; set; }

    // Constructor privado para usar con Factory
    private Appointment() { }

    // Factory Method
    public static Appointment Create(Guid petId, Guid clientId, DateTime startDate, TimeSpan duration, AppointmentType type)
    {
        if (startDate < DateTime.UtcNow.AddHours(24))
            throw new DomainException("Las citas deben programarse con al menos 24 horas de anticipación.");

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

    // Comportamiento
    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
            throw new DomainException("No se puede cancelar una cita ya completada.");
        Status = AppointmentStatus.Cancelled;
    }

    public void Complete()
    {
        Status = AppointmentStatus.Completed;
    }

    public void Reschedule(DateTime newStartDate)
    {
        if (newStartDate < DateTime.UtcNow.AddHours(24)) // Same rule as creation
            throw new DomainException("Las citas deben reprogramarse con al menos 24 horas de anticipación.");
        if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Cancelled)
            throw new DomainException($"No se puede reprogramar una cita en estado '{Status}'.");

        StartDate = newStartDate;
        // Optionally, reset status to Pending if it was Confirmed, etc.
        // Status = AppointmentStatus.Pending;
        // Add UpdatedAt = DateTime.UtcNow; if you have that property
    }

    public void ChangeStatus(AppointmentStatus newStatus)
    {
        // Add any specific logic for status transitions if needed
        // For example, cannot change from Completed to Pending, etc.
        if (Status == AppointmentStatus.Completed && newStatus != AppointmentStatus.Completed)
             throw new DomainException("No se puede cambiar el estado de una cita ya completada a un estado anterior.");
        if (Status == AppointmentStatus.Cancelled && newStatus != AppointmentStatus.Cancelled)
             throw new DomainException("No se puede cambiar el estado de una cita ya cancelada.");

        Status = newStatus;
        // Add UpdatedAt = DateTime.UtcNow; if you have that property
    }
}

// Ensure IAggregateRoot is defined if not already in a shared kernel, or remove if not used.
// For this context, assuming it's a marker interface. If it's from a library, ensure using directive.
public interface IAggregateRoot
{
}