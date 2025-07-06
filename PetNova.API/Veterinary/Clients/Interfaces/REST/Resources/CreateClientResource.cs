using System.ComponentModel.DataAnnotations;

namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

public record CreateClientResource(
    [Required] string FirstName,
    [Required] string LastName,
    [Required][EmailAddress] string Email,
    [Required] string Phone,
    [Required] string Address, // Added Address
    [Required][MinLength(6)] string Password // Added Password
);