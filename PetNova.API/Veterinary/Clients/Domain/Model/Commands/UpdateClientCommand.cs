using System;

namespace PetNova.API.Veterinary.Clients.Domain.Model.Commands;

/// <summary>
///     Command to update an existing client.
/// </summary>
public record UpdateClientCommand(Guid ClientId, string FirstName, string LastName, string Phone);