namespace PetNova.API.Veterinary.Doctor.Domain.Model.Commands;

public record UpdateProfileCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Specialty,
    string Biography);