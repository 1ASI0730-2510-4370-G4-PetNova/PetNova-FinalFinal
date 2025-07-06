using PetNova.API.Veterinary.Pets.Domain.Model.ValueObjects;
using Pet = PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Pets.Domain.Model.Commands;


/// <summary>
///     Command to create a pet.
/// </summary>
/// <param name="Name">
///     The name of the pet.
/// </param>
/// <param name="Breed">
///     The breed of the pet.
/// </param>
/// <param name="DateOfBirth">
///     The date of birth of the pet.
/// </param>
/// <param name="DateRegistered">
///     The registration date of the pet.
/// </param>
/// <param name="Gender">
///     The gender of the pet.
/// </param>
/// <param name="ClientId">
///     The ID of the client who owns the pet.
/// </param>
public record CreatePetCommand(
    string Name,
    string Breed,
    DateTime DateOfBirth,
    DateTime DateRegistered,
    EGender Gender,
    Guid ClientId
);