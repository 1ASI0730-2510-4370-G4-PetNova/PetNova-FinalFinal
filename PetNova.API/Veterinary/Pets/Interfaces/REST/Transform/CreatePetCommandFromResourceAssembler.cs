    using PetNova.API.Veterinary.Pets.Domain.Model.Commands;
    using PetNova.API.Veterinary.Pets.Interfaces.REST.Resources;
    namespace PetNova.API.Veterinary.Pets.Interfaces.REST.Transform;

    /// <summary>
    ///     Interface for Pet command service.
    /// </summary>
    /// <remarks>
    ///     This interface defines the operations related to creating, updating and deleting pets.
    /// </remarks>
    public static class CreatePetCommandFromResourceAssembler
    {
        public static CreatePetCommand ToCommandFromResource(CreatePetResource resource)
        {
            return new CreatePetCommand(
                resource.Name,
                resource.Breed,
                resource.BirthDate,
                resource.RegistrationDate,
                resource.Gender,
                resource.ClientId);
        }
    }