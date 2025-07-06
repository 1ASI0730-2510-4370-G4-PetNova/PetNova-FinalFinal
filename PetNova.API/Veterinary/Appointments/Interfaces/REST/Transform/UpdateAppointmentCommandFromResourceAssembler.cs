using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects; // Added using for AppointmentStatus
using System; // Required for Guid

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST.Transform
{
    public static class UpdateAppointmentCommandFromResourceAssembler
    {
        public static UpdateAppointmentCommand ToCommandFromResource(Guid appointmentId, UpdateAppointmentResource resource)
        {
            // Assuming AppointmentStatus can be parsed from string.
            // Add error handling or use a more robust conversion if needed.
            AppointmentStatus? status = null; // Now directly use AppointmentStatus
            if (!string.IsNullOrEmpty(resource.NewStatus))
            {
                if (System.Enum.TryParse<AppointmentStatus>(resource.NewStatus, true, out var parsedStatus)) // Now directly use AppointmentStatus
                {
                    status = parsedStatus;
                }
                // else: handle invalid status string if necessary
            }

            return new UpdateAppointmentCommand(
                appointmentId,
                resource.NewStartDate,
                status
            );
        }
    }
}
