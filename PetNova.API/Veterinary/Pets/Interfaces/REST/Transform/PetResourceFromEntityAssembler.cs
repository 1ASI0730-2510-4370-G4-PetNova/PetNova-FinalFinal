using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Interfaces.REST.Resources;

namespace PetNova.API.Veterinary.Pets.Interfaces.REST.Transform;

public static class PetResourceFromEntityAssembler
{
    public static PetResource ToResourceFromEntity(Pet entity)
    {
        return new PetResource(
            entity.Id,
            entity.Name,
            entity.Breed,
            entity.DateOfBirth,
            entity.DateRegistered,
            entity.Gender,
            entity.ClientId);
    }
}