using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;

public class Appointment
{
    public Guid Id { get; private set; }
    public string PetName { get; private set; }
    public Guid ClientId { get; private set; }
    public string ClientName { get; private set; }
    public string ContactNumber { get; private set; }
    public DateTime StartDate { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public AppointmentType Type { get; private set; }

    public Appointment(string petName, Guid clientId, string clientName, string contactNumber, DateTime startDate, AppointmentStatus status, AppointmentType type)
    {
        Id = Guid.NewGuid();
        PetName = petName;
        ClientId = clientId;
        ClientName = clientName;
        ContactNumber = contactNumber;
        StartDate = startDate;
        Status = status;
        Type = type;
    }
    public void Update(
        string petName,
        Guid clientId,
        string clientName,
        string contactNumber,
        DateTime startDate,
        AppointmentStatus status,
        AppointmentType type)
    {
        PetName = petName;
        ClientId = clientId;
        ClientName = clientName;
        ContactNumber = contactNumber;
        StartDate = startDate;
        Status = status;
        Type = type;
    }

    public Appointment(CreateAppointmentCommand command)
    {
        Id = Guid.NewGuid();
        PetName = command.PetName;
        ClientId = command.ClientId;
        ClientName = command.ClientName;
        ContactNumber = command.ContactNumber;
        StartDate = command.StartDate;
        Status = command.Status;
        Type = command.Type;
    }

}
