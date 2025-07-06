namespace PetNova.API.Veterinary.Doctor.Domain.Model.Queries;

public interface IProfileQueryService
{
    Task<ProfileResponse> Execute(GetProfileQuery query);
    Task<ProfilePictureResponse> Execute(GetProfilePictureQuery query);
}