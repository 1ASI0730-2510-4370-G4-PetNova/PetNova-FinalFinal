using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;

namespace PetNova.API.Veterinary.Appointments.Domain.Repositories;

// Ubicación sugerida: PetNova.API.Veterinary.Appoiments.Domain.Repositories
public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task AddAsync(Appointment appointment);
    void Update(Appointment appointment);
    void Remove(Appointment appointment);
}
