namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

public record CreateClientResource(
    string FirstName,
    string LastName,
    string Email,
    string Phone);