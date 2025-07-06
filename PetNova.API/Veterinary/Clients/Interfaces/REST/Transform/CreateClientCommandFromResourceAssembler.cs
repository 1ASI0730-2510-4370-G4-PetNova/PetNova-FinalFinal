using PetNova.API.Veterinary.Clients.Domain.Model.Commands;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Transform;

public static class CreateClientCommandFromResourceAssembler
{
    public static RegisterClientCommand ToCommandFromResource(CreateClientResource resource)
    {
        return new RegisterClientCommand(
            resource.FirstName,
            resource.LastName,
            resource.Email,
            resource.Phone);
    }
}