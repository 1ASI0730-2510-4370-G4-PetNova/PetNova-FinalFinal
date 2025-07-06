namespace PetNova.API.Veterinary.Clients.Domain.Services;

using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;

public interface IClientQueryService
{
    Task<Client?> GetByIdAsync(Guid clientId);
    Task<IEnumerable<Client>> ListAsync();
}