using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Interfaces.DTOs;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Application.Services;

public class AppointmentService
{
    private readonly IRepository<Appointment, Guid> _appointmentRepository;
    private readonly IRepository<Pet, Guid> _petRepository;
    private readonly IRepository<Doctor, Guid> _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(
        IRepository<Appointment, Guid> appointmentRepository,
        IRepository<Pet, Guid> petRepository,
        IRepository<Doctor, Guid> doctorRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _petRepository = petRepository;
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Appointment>> ListAsync()
    {
        return await _appointmentRepository.ListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await _appointmentRepository.FindByIdAsync(id);
    }

    public async Task<IEnumerable<Appointment>> GetByPetIdAsync(Guid petId)
    {
        var pet = await _petRepository.FindByIdAsync(petId);
        return pet?.Appointments ?? Enumerable.Empty<Appointment>();
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(Guid doctorId)
    {
        var doctor = await _doctorRepository.FindByIdAsync(doctorId);
        return doctor?.Appointments ?? Enumerable.Empty<Appointment>();
    }

    public async Task<Appointment> CreateAsync(AppointmentDto appointmentDto)
    {
        var appointment = new Appointment
        {
            AppointmentDate = appointmentDto.AppointmentDate,
            Reason = appointmentDto.Reason,
            Status = appointmentDto.Status,
            Notes = appointmentDto.Notes,
            PetId = appointmentDto.PetId,
            DoctorId = appointmentDto.DoctorId
        };
        
        await _appointmentRepository.AddAsync(appointment);
        await _unitOfWork.CompleteAsync();
        
        return appointment;
    }

    public async Task<Appointment?> UpdateAsync(Guid id, AppointmentDto appointmentDto)
    {
        var existingAppointment = await _appointmentRepository.FindByIdAsync(id);
        if (existingAppointment == null) return null;

        existingAppointment.AppointmentDate = appointmentDto.AppointmentDate;
        existingAppointment.Reason = appointmentDto.Reason;
        existingAppointment.Status = appointmentDto.Status;
        existingAppointment.Notes = appointmentDto.Notes;
        existingAppointment.UpdatedAt = DateTime.UtcNow;

        _appointmentRepository.Update(existingAppointment);
        await _unitOfWork.CompleteAsync();
        
        return existingAppointment;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var appointment = await _appointmentRepository.FindByIdAsync(id);
        if (appointment == null) return false;

        _appointmentRepository.Remove(appointment);
        await _unitOfWork.CompleteAsync();
        
        return true;
    }
}