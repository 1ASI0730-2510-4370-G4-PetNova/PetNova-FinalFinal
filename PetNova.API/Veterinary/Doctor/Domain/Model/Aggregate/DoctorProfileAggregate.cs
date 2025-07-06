using PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Doctor.Domain.Model.Commands;

namespace PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;

public class DoctorProfileAggregate
{
    public Guid Id { get; private set; }
    public FullName Name { get; private set; }
    public Specialty Specialty { get; private set; }
    public Biography Biography { get; private set; }
    public string ProfilePictureUrl { get; private set; }

    // Constructor privado para factory methods
    private DoctorProfileAggregate() { }

    // Factory method para creación
    public static DoctorProfileAggregate Create(
        FullName name,
        Specialty specialty,
        Biography biography,
        string profilePictureUrl = "")
    {
        return new DoctorProfileAggregate
        {
            Id = Guid.NewGuid(),
            Name = name,
            Specialty = specialty,
            Biography = biography,
            ProfilePictureUrl = profilePictureUrl
        };
    }

    // Métodos de negocio
    public void UpdateProfile(FullName name, Specialty specialty, Biography biography)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
        Biography = biography ?? throw new ArgumentNullException(nameof(biography));
    }

    public void UpdateProfilePicture(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));
        
        ProfilePictureUrl = imageUrl;
    }

    public void DeleteProfile()
    {
        // Lógica para marcado como eliminado o limpieza
        ProfilePictureUrl = string.Empty;
    }
}

public static class DoctorProfileAggregateExtensions
{
    public static DoctorProfileAggregate ToAggregate(this UpdateProfileCommand command)
    {
        var name = new FullName(command.FirstName, command.LastName);
        var specialty = new Specialty(command.Specialty);
        var biography = new Biography(command.Biography);
        
        var aggregate = DoctorProfileAggregate.Create(
            name,
            specialty,
            biography);
        
        return aggregate;
    }
}