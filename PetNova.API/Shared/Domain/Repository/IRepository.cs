using Pet = PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Domain.Repository;

/// <summary>
///     Base repository interface for all repositories
/// </summary>
/// <remarks>
///     This interface defines the basic CRUD operations for all repositories
/// </remarks>
/// <typeparam name="TEntity">The Entity Type</typeparam>
public interface IBaseRepository<TEntity, TId>
{
    Task AddAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(TId id);
    Task<IEnumerable<TEntity>> ListAsync();
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
public interface IRepository<TEntity, TId>
{
    Task AddAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(TId id);
    Task<IEnumerable<TEntity>> ListAsync();
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> Exists(TId id);
}
