namespace PetNova.API.Veterinary.Pets.Domain.Model.Commands;
/// <summary>
///     Command to delete a pet.
/// </summary>
/// <param name="PetId">
///     The ID of the pet to delete.
/// </param>
public record DeletePetCommand(Guid PetId);