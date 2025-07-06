using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Model.Commands;

namespace PetNova.API.Veterinary.Doctor.Domain.Repositories;

public interface IProfileCommandService
{
    /// <summary>
    /// Creates a new doctor profile
    /// </summary>
    /// <param name="profile">Doctor profile aggregate to create</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task AddProfileAsync(DoctorProfileAggregate profile);
    
    /// <summary>
    /// Updates an existing doctor profile
    /// </summary>
    /// <param name="profile">Updated doctor profile aggregate</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task UpdateProfileAsync(DoctorProfileAggregate profile);
    
    /// <summary>
    /// Deletes a doctor profile by ID
    /// </summary>
    /// <param name="id">Unique identifier of the profile to delete</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task DeleteProfileAsync(Guid id);
    
    /// <summary>
    /// Updates the profile picture for a doctor
    /// </summary>
    /// <param name="id">Doctor profile ID</param>
    /// <param name="imageUrl">URL of the new profile picture</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task UpdateProfilePictureAsync(Guid id, string imageUrl);
    
    /// <summary>
    /// Creates a new profile from a command object
    /// </summary>
    /// <param name="command">Command containing profile data</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task CreateProfileFromCommandAsync(UpdateProfileCommand command);
    
    /// <summary>
    /// Updates a profile from a command object
    /// </summary>
    /// <param name="command">Command containing updated profile data</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task UpdateProfileFromCommandAsync(UpdateProfileCommand command);
    
    /// <summary>
    /// Checks if a profile exists by ID
    /// </summary>
    /// <param name="id">Profile ID to check</param>
    /// <returns>True if the profile exists, false otherwise</returns>
    Task<bool> ProfileExistsAsync(Guid id);
}