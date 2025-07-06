namespace PetNova.API.Veterinary.Clients.Domain.Services;

using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;


public interface IClientCommandService
{
    Task<Client> CreateAsync(Client client);
    Task<Client?> UpdateAsync(Guid clientId, Client updatedClient);
    Task<bool> DeleteAsync(Guid clientId);
}