using System; // Required for Guid

namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

public record ClientResource(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Address, // Added Address based on Client entity
    DateTime CreatedAt, // Added for consistency
    DateTime? UpdatedAt // Added for consistency
);
