namespace PetNova.API.Veterinary.Doctor.Domain.Repositories;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;

public interface IProfileRepository
{
    Task<DoctorProfileAggregate?> GetByIdAsync(Guid id);
    Task AddAsync(DoctorProfileAggregate profile);
    Task UpdateAsync(DoctorProfileAggregate profile);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}