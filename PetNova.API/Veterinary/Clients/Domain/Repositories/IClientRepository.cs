using PetNova.API.Shared.Domain.Repository;
using ClientEntity = PetNova.API.Veterinary.Clients.Domain.Model.Aggregate.Client;

namespace PetNova.API.Veterinary.Clients.Domain.Repositories;

/// <summary>
///     Interface for the Client repository, abstracting the persistence logic.
/// </summary>
public interface IClientRepository : IBaseRepository<ClientEntity, Guid>
{
    Task<List<ClientEntity>> ListAsync();
    Task<ClientEntity?> FindByIdAsync(Guid id);
    Task<ClientEntity?> GetByIdAsync(Guid id);
    Task AddAsync(ClientEntity client);
    Task UpdateAsync(ClientEntity client);
    Task DeleteAsync(ClientEntity client);
}
