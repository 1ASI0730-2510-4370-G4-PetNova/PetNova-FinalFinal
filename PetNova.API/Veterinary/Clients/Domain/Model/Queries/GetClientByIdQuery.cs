namespace PetNova.API.Veterinary.Clients.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a client by its ID.
/// </summary>
public record GetClientByIdQuery(Guid ClientId);