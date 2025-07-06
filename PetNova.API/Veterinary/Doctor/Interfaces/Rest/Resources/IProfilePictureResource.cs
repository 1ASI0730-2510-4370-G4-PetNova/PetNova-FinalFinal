namespace PetNova.API.Veterinary.Doctor.Interfaces.Rest.Resources;

public interface IProfilePictureResource
{
    string ImageUrl { get; }
    DateTime UploadDate { get; }
}