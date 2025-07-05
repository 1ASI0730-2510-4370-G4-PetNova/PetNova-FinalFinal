using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Pets.Domain.Repositories;

/// <summary>
///     The Pet repository contract.
/// </summary>
public interface IPetRepository : IBaseRepository<Pet,Guid>
{
    /// <summary>
    ///     Find all pets that belong to a specific client.
    /// </summary>
    /// <param name="clientId">The client ID</param>
    /// <returns>
    ///     A list of pets owned by the specified client.
    /// </returns>
    Task<IEnumerable<Pet>> FindByClientIdAsync(Guid clientId);

    /// <summary>
    ///     Find a specific pet by its unique ID.
    /// </summary>
    /// <param name="id">The pet's ID</param>
    /// <returns>
    ///     The pet object if found, or null otherwise.
    /// </returns>
   Task<Pet?> FindByIdAsync(Guid id);
    Task<IEnumerable<Pet>> ListAsync();

    Task AddAsync(Pet pet);
    void Update(Pet pet);

    Task UpdateAsync(Pet pet);
    Task DeleteAsync(Pet pet);
}