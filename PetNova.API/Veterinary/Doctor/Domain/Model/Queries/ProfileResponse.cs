namespace PetNova.API.Veterinary.Doctor.Domain.Model.Queries;

public record ProfileResponse(
    Guid Id,
    string FullName,
    string Specialty,
    string Biography,
    string ProfilePictureUrl);