using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest.Transform;

public static class CommandTransform
{
    public static CreateAppointmentCommand ToCommand(this CreateAppointmentResource resource)
    {
        return new CreateAppointmentCommand(
            resource.PetId,
            resource.ClientId,
            resource.StartDate,
            resource.Duration,
            Enum.Parse<AppointmentType>(resource.Type));
    }
}