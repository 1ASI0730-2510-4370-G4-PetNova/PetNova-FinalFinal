namespace PetNova.API.Veterinary.Pets.Domain.Model.Queries;

/// <summary>
///     Query to get a pet by its ID
/// </summary>
/// <param name="PetId">The ID of the pet to retrieve</param>
public record GetPetByIdQuery(Guid PetId);