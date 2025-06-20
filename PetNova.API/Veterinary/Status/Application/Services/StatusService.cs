using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Status.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Status.Interface.DTOs;

namespace PetNova.API.Veterinary.Status.Application.Services;

public abstract class StatusService
{
    private readonly IRepository<Domain.Model.Aggregate.Status, Guid> _statusRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StatusService(IRepository<Domain.Model.Aggregate.Status, Guid> statusRepository, IUnitOfWork unitOfWork)
    {
        _statusRepository = statusRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Domain.Model.Aggregate.Status>> ListAsync()
    {
        return await _statusRepository.ListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregate.Status>> ListByTypeAsync(string type)
    {
        var allStatuses = await _statusRepository.ListAsync();
        return allStatuses.Where(s => s.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Domain.Model.Aggregate.Status?> GetByIdAsync(Guid id)
    {
        return await _statusRepository.FindByIdAsync(id);
    }

    public async Task<Domain.Model.Aggregate.Status> CreateAsync(StatusDTO statusDto)
    {
        var status = new Domain.Model.Aggregate.Status
        {
            Name = statusDto.Name,
            Description = statusDto.Description,
            Type = statusDto.Type,
            IsActive = statusDto.IsActive
        };
        
        await _statusRepository.AddAsync(status);
        await _unitOfWork.CompleteAsync();
        
        return status;
    }

    public async Task<Domain.Model.Aggregate.Status?> UpdateAsync(Guid id, StatusDTO statusDto)
    {
        var existingStatus = await _statusRepository.FindByIdAsync(id);
        if (existingStatus == null) return null;

        existingStatus.Name = statusDto.Name;
        existingStatus.Description = statusDto.Description;
        existingStatus.Type = statusDto.Type;
        existingStatus.IsActive = statusDto.IsActive;
        existingStatus.UpdatedAt = DateTime.UtcNow;

        _statusRepository.Update(existingStatus);
        await _unitOfWork.CompleteAsync();
        
        return existingStatus;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var status = await _statusRepository.FindByIdAsync(id);
        if (status == null) return false;

        _statusRepository.Remove(status);
        await _unitOfWork.CompleteAsync();
        
        return true;
    }
}