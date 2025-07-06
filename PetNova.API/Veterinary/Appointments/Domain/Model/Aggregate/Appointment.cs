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

    public void Reschedule(DateTime commandNewStartDate)
    {
        throw new NotImplementedException();
    }

    public void ChangeStatus(AppointmentStatus commandNewStatus)
    {
        throw new NotImplementedException();
    }
}

public interface IAggregateRoot
{
}