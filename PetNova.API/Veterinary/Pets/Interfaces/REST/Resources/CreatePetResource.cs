using PetNova.API.Veterinary.Pets.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Pets.Interfaces.REST.Resources;

public record CreatePetResource(
    string Name,
    string Breed,
    DateTime BirthDate,
    DateTime RegistrationDate,
    EGender Gender,
    Guid ClientId);