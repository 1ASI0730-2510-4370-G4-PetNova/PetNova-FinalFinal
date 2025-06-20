using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.ClientAndPetManagement.Domain.Model.Aggregate;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;

public class PetService
{
    private readonly IRepository<Pet, Guid> _petRepository;
    private readonly IRepository<Client, Guid> _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PetService(
        IRepository<Pet, Guid> petRepository,
        IRepository<Client, Guid> clientRepository,
        IUnitOfWork unitOfWork)
    {
        _petRepository = petRepository;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Pet>> ListAsync()
    {
        return await _petRepository.ListAsync();
    }

    public async Task<Pet?> GetByIdAsync(Guid id)
    {
        return await _petRepository.FindByIdAsync(id);
    }

    public async Task<IEnumerable<Pet>> GetByClientIdAsync(Guid clientId)
    {
        var client = await _clientRepository.FindByIdAsync(clientId);
        return client?.Pets ?? Enumerable.Empty<Pet>();
    }

    public async Task<Pet> CreateAsync(PetDTO petDto)
    {
        var pet = new Pet
        {
            Name = petDto.Name,
            Species = petDto.Species,
            Breed = petDto.Breed,
            DateOfBirth = petDto.DateOfBirth,
            Color = petDto.Color,
            Gender = petDto.Gender,
            MicrochipId = petDto.MicrochipId,
            ClientId = petDto.ClientId
        };
        
        await _petRepository.AddAsync(pet);
        await _unitOfWork.CompleteAsync();
        
        return pet;
    }

    public async Task<Pet?> UpdateAsync(Guid id, PetDTO petDto)
    {
        var existingPet = await _petRepository.FindByIdAsync(id);
        if (existingPet == null) return null;

        existingPet.Name = petDto.Name;
        existingPet.Species = petDto.Species;
        existingPet.Breed = petDto.Breed;
        existingPet.DateOfBirth = petDto.DateOfBirth;
        existingPet.Color = petDto.Color;
        existingPet.Gender = petDto.Gender;
        existingPet.MicrochipId = petDto.MicrochipId;
        existingPet.UpdatedAt = DateTime.UtcNow;

        _petRepository.Update(existingPet);
        await _unitOfWork.CompleteAsync();
        
        return existingPet;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var pet = await _petRepository.FindByIdAsync(id);
        if (pet == null) return false;

        _petRepository.Remove(pet);
        await _unitOfWork.CompleteAsync();
        
        return true;
    }
}