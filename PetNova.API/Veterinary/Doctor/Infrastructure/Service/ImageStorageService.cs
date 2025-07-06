namespace PetNova.API.Veterinary.Doctor.Infrastructure.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetNova.API.Veterinary.Doctor.Domain.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImageStorageService> _logger;
    private const string ProfileImagesFolder = "profile-images";

    public ImageStorageService(
        IWebHostEnvironment env,
        ILogger<ImageStorageService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        try
        {
            // Validaciones
            if (imageStream == null || imageStream.Length == 0)
                throw new ArgumentException("No image stream provided");

            if (imageStream.Length > 5_000_000) // 5MB
                throw new ArgumentException("Image size exceeds maximum allowed (5MB)");

            // Crear directorio si no existe
            var uploadsPath = Path.Combine(_env.WebRootPath, ProfileImagesFolder);
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Generar nombre único
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // Guardar archivo
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await imageStream.CopyToAsync(fileStream);

            // Retornar URL relativa
            return $"/{ProfileImagesFolder}/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading profile image");
            throw;
        }
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return;

            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_env.WebRootPath, ProfileImagesFolder, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting profile image {ImageUrl}", imageUrl);
            throw;
        }
    }
}