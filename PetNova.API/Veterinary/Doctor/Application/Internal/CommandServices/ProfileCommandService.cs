using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;
using PetNova.API.Veterinary.Doctor.Domain.Model.Commands;
using PetNova.API.Veterinary.Doctor.Infrastructure.Service;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;
using Microsoft.Extensions.Logging;

namespace PetNova.API.Veterinary.Doctor.Application.Internal.CommandServices;

public class ProfileCommandService : IProfileCommandService
{
    private readonly IProfileRepository _repository;
    private readonly IImageStorageService _imageService;
    private readonly ILogger<ProfileCommandService> _logger;

    public ProfileCommandService(
        IProfileRepository repository, 
        IImageStorageService imageService,
        ILogger<ProfileCommandService> logger)
    {
        _repository = repository;
        _imageService = imageService;
        _logger = logger;
    }
    
    public async Task AddProfileAsync(DoctorProfileAggregate profile)
    {
        try
        {
            await _repository.AddAsync(profile);
            _logger.LogInformation("Profile added successfully with ID {ProfileId}", profile.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new profile");
            throw;
        }
    }
    
    public async Task UpdateProfileAsync(DoctorProfileAggregate profile)
    {
        try
        {
            // Verificar existencia primero
            if (!await _repository.ExistsAsync(profile.Id))
            {
                _logger.LogWarning("Profile not found for update: {ProfileId}", profile.Id);
                throw new KeyNotFoundException($"Profile with ID {profile.Id} not found");
            }

            await _repository.UpdateAsync(profile);
            _logger.LogInformation("Profile updated successfully: {ProfileId}", profile.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating profile {ProfileId}", profile.Id);
            throw; // Relanzar para que el Controller lo maneje
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile {ProfileId}", profile.Id);
            throw;
        }
    }
    
    public async Task DeleteProfileAsync(Guid id)
    {
        try
        {
            // Verificar existencia primero
            if (!await _repository.ExistsAsync(id))
            {
                _logger.LogWarning("Profile not found for deletion: {ProfileId}", id);
                throw new KeyNotFoundException($"Profile with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            _logger.LogInformation("Profile deleted successfully: {ProfileId}", id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict deleting profile {ProfileId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting profile {ProfileId}", id);
            throw;
        }
    }
    
    public async Task<bool> ProfileExistsAsync(Guid id)
    {
        try
        {
            return await _repository.ExistsAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if profile exists: {ProfileId}", id);
            throw;
        }
    }

    public async Task UpdateProfilePictureAsync(Guid id, string imageUrl)
    {
        try
        {
            var profile = await _repository.GetByIdAsync(id);
            if (profile == null)
            {
                _logger.LogWarning("Profile not found for picture update: {ProfileId}", id);
                throw new KeyNotFoundException($"Profile with ID {id} not found");
            }

            profile.UpdateProfilePicture(imageUrl);
            await _repository.UpdateAsync(profile);
            _logger.LogInformation("Profile picture updated successfully: {ProfileId}", id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating profile picture {ProfileId}", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile picture {ProfileId}", id);
            throw;
        }
    }

    public async Task CreateProfileFromCommandAsync(UpdateProfileCommand command)
    {
        try
        {
            var profile = command.ToAggregate();
            await _repository.AddAsync(profile);
            _logger.LogInformation("Profile created from command successfully: {ProfileId}", profile.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating profile from command");
            throw;
        }
    }
    
    public async Task UpdateProfileFromCommandAsync(UpdateProfileCommand command)
    {
        try
        {
            var existingProfile = await _repository.GetByIdAsync(command.Id);
            if (existingProfile == null)
            {
                _logger.LogWarning("Profile not found for command update: {ProfileId}", command.Id);
                throw new KeyNotFoundException($"Profile with ID {command.Id} not found");
            }

            var name = new FullName(command.FirstName, command.LastName);
            var specialty = new Specialty(command.Specialty);
            var biography = new Biography(command.Biography);
        
            existingProfile.UpdateProfile(name, specialty, biography);
            await _repository.UpdateAsync(existingProfile);
            _logger.LogInformation("Profile updated from command successfully: {ProfileId}", command.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating profile from command {ProfileId}", command.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile from command {ProfileId}", command.Id);
            throw;
        }
    }
}