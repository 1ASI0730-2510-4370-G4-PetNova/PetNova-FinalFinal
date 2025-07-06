using System.ComponentModel.DataAnnotations;

namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

public record UpdateClientResource(
    string? FirstName,
    string? LastName,
    [EmailAddress] string? Email,
    string? Phone,
    string? Address
    // Password updates should typically be handled via a separate, more secure endpoint
);