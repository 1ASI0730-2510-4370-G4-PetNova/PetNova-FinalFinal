namespace PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;

public record Specialty(string Value)
{
    public static Specialty Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Specialty cannot be empty", nameof(value));
        
        return new Specialty(value.Trim());
    }

    public static implicit operator string(Specialty specialty) => specialty.Value;
}