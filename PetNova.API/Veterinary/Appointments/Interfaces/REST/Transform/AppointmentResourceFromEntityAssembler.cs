using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Transform
{
    public static class AppointmentResourceFromEntityAssembler
    {
        public static AppointmentResource ToResourceFromEntity(Appointment entity)
        {
            return new AppointmentResource(
                entity.Id,
                entity.PetId,
                entity.ClientId,
                entity.StartDate,
                (int)entity.Duration.TotalMinutes, // Convert TimeSpan to int (total minutes)
                entity.Type.ToString(), // Convert AppointmentType enum to string
                entity.Status.ToString(),
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }
    }
}
