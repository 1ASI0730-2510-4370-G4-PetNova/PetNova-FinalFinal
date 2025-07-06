namespace PetNova.API.Veterinary.Doctor.Infrastructure.Service;
public interface IImageStorageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName);
    Task DeleteImageAsync(string imageUrl);
}
