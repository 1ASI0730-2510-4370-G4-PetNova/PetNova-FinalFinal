namespace PetNova.API.Veterinary.Doctor.Domain.Services;
using PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;


public interface IProfileDomainService
{
    Task ValidateProfileUniquenessAsync(FullName name, Specialty specialty);
    Task HandleProfilePictureUpdateAsync(Guid profileId, string newImageUrl);
}