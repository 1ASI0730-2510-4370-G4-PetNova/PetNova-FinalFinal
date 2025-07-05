using PetNova.API.Veterinary.Pets.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Pets.Interfaces.REST.Resources;

public record PetResource(
    Guid Id,
    string Name,
    string Breed,
    DateTime DateOfBirth,
    DateTime DateRegistered,
    EGender Gender,
    Guid ClientId);