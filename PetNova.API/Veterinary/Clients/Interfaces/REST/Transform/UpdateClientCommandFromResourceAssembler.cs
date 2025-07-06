using PetNova.API.Veterinary.Clients.Domain.Model.Commands;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;
using System;

namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Transform
{
    public static class UpdateClientCommandFromResourceAssembler
    {
        public static UpdateClientCommand ToCommandFromResource(Guid clientId, UpdateClientResource resource)
        {
            return new UpdateClientCommand(
                clientId,
                resource.FirstName,
                resource.LastName,
                resource.Email,
                resource.Phone,
                resource.Address
            );
        }
    }
}
