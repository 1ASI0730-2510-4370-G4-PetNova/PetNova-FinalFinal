namespace PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;

public record Biography(string Content)
{
    public static Biography Create(string content)
    {
        return new Biography(content?.Trim() ?? string.Empty);
    }

    public static implicit operator string(Biography bio) => bio.Content;
}