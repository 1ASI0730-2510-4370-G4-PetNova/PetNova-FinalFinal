using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Clients.Domain.Repositories;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Repositories;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using ClientEntity = PetNova.API.Veterinary.Clients.Domain.Model.Aggregate.Client;

namespace PetNova.API.Veterinary.Clients.Infrastructure.Repositories;

/// <summary>
///     Implementation of the client repository using Entity Framework Core.
/// </summary>
public class ClientRepository : EfRepository<ClientEntity, Guid>, IClientRepository
{
    public ClientRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<ClientEntity>> ListAsync()
    {
        return await Context.Set<ClientEntity>().ToListAsync();
    }

    public async Task<ClientEntity?> FindByIdAsync(Guid id)
    {
        return await Context.Set<ClientEntity>().FindAsync(id);
    }

    public async Task<ClientEntity?> GetByIdAsync(Guid id)
    {
        return await FindByIdAsync(id);
     }

    public async Task AddAsync(ClientEntity client)
    {
        await Context.Set<ClientEntity>().AddAsync(client);
    }

    public async Task UpdateAsync(ClientEntity client)
    {
        Context.Set<ClientEntity>().Update(client);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(ClientEntity client)
    {
        Context.Set<ClientEntity>().Remove(client);
        await Task.CompletedTask;
    }
}