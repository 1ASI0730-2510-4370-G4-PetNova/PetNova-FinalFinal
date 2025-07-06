namespace PetNova.API.Veterinary.Clients.Domain.Model.ValueObjects;

/// <summary>
///     Represents a full name composed of first and last names.
/// </summary>
/// <param name="FirstName">The first name of the client.</param>
/// <param name="LastName">The last name of the client.</param>
public record FullName(string FirstName, string LastName);