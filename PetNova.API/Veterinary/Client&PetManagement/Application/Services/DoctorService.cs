using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public class DoctorService
{
    private readonly IRepository<Doctor, Guid> _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IRepository<Doctor, Guid> doctorRepository, IUnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Doctor>> ListAsync()
    {
        return await _doctorRepository.ListAsync();
    }

    public async Task<Doctor?> GetByIdAsync(Guid id)
    {
        return await _doctorRepository.FindByIdAsync(id);
    }

    public async Task<Doctor> CreateAsync(DoctorDTO doctorDto)
    {
        var doctor = new Doctor
        {
            FirstName = doctorDto.FirstName,
            LastName = doctorDto.LastName,
            Specialization = doctorDto.Specialization,
            LicenseNumber = doctorDto.LicenseNumber,
            Email = doctorDto.Email,
            Phone = doctorDto.Phone
        };
        
        await _doctorRepository.AddAsync(doctor);
        await _unitOfWork.CompleteAsync();
        
        return doctor;
    }

    public async Task<Doctor?> UpdateAsync(Guid id, DoctorDTO doctorDto)
    {
        var existingDoctor = await _doctorRepository.FindByIdAsync(id);
        if (existingDoctor == null) return null;

        existingDoctor.FirstName = doctorDto.FirstName;
        existingDoctor.LastName = doctorDto.LastName;
        existingDoctor.Specialization = doctorDto.Specialization;
        existingDoctor.LicenseNumber = doctorDto.LicenseNumber;
        existingDoctor.Email = doctorDto.Email;
        existingDoctor.Phone = doctorDto.Phone;
        existingDoctor.UpdatedAt = DateTime.UtcNow;

        _doctorRepository.Update(existingDoctor);
        await _unitOfWork.CompleteAsync();
        
        return existingDoctor;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var doctor = await _doctorRepository.FindByIdAsync(id);
        if (doctor == null) return false;

        _doctorRepository.Remove(doctor);
        await _unitOfWork.CompleteAsync();
        
        return true;
    }
}