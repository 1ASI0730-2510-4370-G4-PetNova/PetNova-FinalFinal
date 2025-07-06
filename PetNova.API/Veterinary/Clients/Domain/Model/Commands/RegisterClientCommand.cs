namespace PetNova.API.Veterinary.Clients.Domain.Model.Commands;

/// <summary>
///     Command to register a new client.
/// </summary>
public record RegisterClientCommand(string FirstName, string LastName, string Email, string Phone);