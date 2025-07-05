namespace PetNova.API.Veterinary.Pets.Domain.Model.Queries;

/// <summary>
///     Query to get all pets by client ID
/// </summary>
/// <param name="ClientId">The client ID to search pets for</param>
public record GetAllPetsByClientIdQuery(Guid ClientId);
