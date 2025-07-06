using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Appointments.Domain.Repositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(Guid id);
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status);
    Task<IEnumerable<Appointment>> GetAllAsync(int page, int pageSize);
}
