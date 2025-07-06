using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Repositories;

namespace PetNova.API.Veterinary.Pets.Infrastructure.Repositories;

/// <summary>
///     Entity Framework Core implementation of the Pet repository.
/// </summary>
/// <param name="context">
///     The <see cref="AppDbContext"/> used for data access.
/// </param>
public class PetRepository(AppDbContext context)
    : EfRepository<Pet, Guid>(context), IPetRepository
{
    public async Task<IEnumerable<Pet>> FindByClientIdAsync(Guid clientId)
    {
        return await Context.Set<Pet>()
            .Where(p => p.ClientId == clientId)
            .ToListAsync();
    }

    public new async Task<Pet?> FindByIdAsync(Guid id)
    {
        return await Context.Set<Pet>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public new async Task<IEnumerable<Pet>> ListAsync()
    {
        return await Context.Set<Pet>().ToListAsync();
    }
    public async Task UpdateAsync(Pet pet)
    {
        Context.Set<Pet>().Update(pet);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pet pet)
    {
        Context.Set<Pet>().Remove(pet);
        await Context.SaveChangesAsync();
    }

}

