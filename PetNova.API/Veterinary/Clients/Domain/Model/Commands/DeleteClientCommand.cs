namespace PetNova.API.Veterinary.Clients.Domain.Model.Commands;

/// <summary>
///     Command to delete a client.
/// </summary>
public record DeleteClientCommand(Guid ClientId);