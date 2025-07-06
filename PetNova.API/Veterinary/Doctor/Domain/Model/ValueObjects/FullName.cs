namespace PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;
public record FullName(string FirstName, string LastName)
{
    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        
        return new FullName(firstName.Trim(), lastName.Trim());
    }

    public override string ToString() => $"{FirstName} {LastName}";
}