namespace PetNova.API.Veterinary.Doctor.Interfaces.Rest.Resources;

public interface IProfileResource
{
    string FullName { get; }
    string Specialty { get; }
    string Biography { get; }
    string ProfilePictureUrl { get; }
}