using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Commands;
using Pet = PetNova.API.Veterinary.Pets.Domain.Model.Aggregate.Pet;

namespace PetNova.API.Veterinary.Pets.Domain.Services;

/// <summary>
///     Interface for Pet command service.
/// </summary>
/// <remarks>
///     This interface defines the operations related to creating, updating and deleting pets.
/// </remarks>
public interface IPetDomainCommandService
{
    /// <summary>
    ///     Handle the create pet command.
    /// </summary>
    /// <param name="command">The CreatePetCommand</param>
    /// <returns>The created Pet aggregate or null if failed</returns>
    Task<Pet?> Handle(CreatePetCommand command);

    /// <summary>
    ///     Handle the update pet command.
    /// </summary>
    /// <param name="id">The ID of the pet to update</param>
    /// <param name="command">The updated values</param>
    /// <returns>The updated Pet aggregate or null if not found</returns>
 ////   Task<Pet?> Handle(Guid id, CreatePetCommand command);

    /// <summary>
    ///     Handle the delete pet command.
    /// </summary>
    /// <param name="id">The ID of the pet to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
  /////  Task<bool> Handle(Guid id);
    Task<bool> Handle(DeletePetCommand command);
    Task<Pet?> Handle(Guid id, CreatePetCommand command);

}