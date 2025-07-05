using PetNova.API.Veterinary.Pets.Domain.Model.Commands;
using PetNova.API.Veterinary.Pets.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

/// <summary>
///     Pet aggregate root that represents a registered pet.
/// </summary>
public partial class Pet
{
    protected Pet() { }

    /// <summary>
    ///     Initializes a new Pet aggregate from a CreatePetCommand.
    /// </summary>
    public Pet(CreatePetCommand command)
    {
        Id = Guid.NewGuid();
        Name = command.Name;
        Breed = command.Breed;
        DateOfBirth = command.DateOfBirth;
        DateRegistered = command.DateRegistered;
        Gender = command.Gender;
        ClientId = command.ClientId;
    }

    public Guid Id { get; protected set; }
    public string Name { get; set; } = null!;
    public string Breed { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public DateTime DateRegistered { get; set; }
    public EGender Gender { get; set; }
    public Guid ClientId { get; set; }

    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
    }
    public void Update(string name, string breed, DateTime dateOfBirth, DateTime dateRegistered, EGender gender)
    {
        Name = name;
        Breed = breed;
        DateOfBirth = dateOfBirth;
        DateRegistered = dateRegistered;
        Gender = gender;
    }
}