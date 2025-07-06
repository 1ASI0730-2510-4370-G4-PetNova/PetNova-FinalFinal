using ClientEntity = PetNova.API.Veterinary.Clients.Domain.Model.Aggregate.Client;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Transform;

public static class ClientResourceFromEntityAssembler
{
    public static ClientResource ToResourceFromEntity(ClientEntity entity)
    {
        return new ClientResource(
            entity.Id,
            entity.Name.FirstName,
            entity.Name.LastName,
            entity.Email,
            entity.Phone,
            entity.Address,    // Added Address
            entity.CreatedAt,  // Added CreatedAt
            entity.UpdatedAt   // Added UpdatedAt
        );
    }
}
