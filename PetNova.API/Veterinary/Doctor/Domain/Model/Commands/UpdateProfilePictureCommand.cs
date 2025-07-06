namespace PetNova.API.Veterinary.Doctor.Domain.Model.Commands;

public record UpdateProfilePictureCommand(
    Guid Id,
    IFormFile ImageFile);