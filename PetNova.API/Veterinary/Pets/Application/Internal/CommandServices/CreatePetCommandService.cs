    using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
    using PetNova.API.Veterinary.Pets.Domain.Model.Commands;

    using PetNova.API.Veterinary.Pets.Domain.Repositories;
    using PetNova.API.Veterinary.Pets.Domain.Services;
    using PetNova.API.Shared.Domain.Repository;
    using Pet = PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

    namespace PetNova.API.Veterinary.Pets.Application.Internal.CommandServices;

    /// <summary>
    ///     Pet command service.
    /// </summary>
    /// <param name="petRepository">The pet repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public class PetCommandService(
        IPetRepository petRepository,
        IUnitOfWork unitOfWork)
        : IPetDomainCommandService
    {
        public async Task<Pet.Pet?> Handle(CreatePetCommand command)
        {


            var pet = new Pet.Pet(command);

            try
            {
                await petRepository.AddAsync(pet);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                // Loggear si deseas
                return null;
            }

            return pet;
        }

        public async Task<bool> Handle(DeletePetCommand command)
        {
            var pet = await petRepository.FindByIdAsync(command.PetId);
            if (pet is null) return false;

            try
            {
                petRepository.Remove(pet);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<Pet.Pet?> Handle(Guid id, CreatePetCommand command)
        {
            var pet = await petRepository.FindByIdAsync(id);
            if (pet is null) return null;

            pet.Update(command.Name, command.Breed, command.DateOfBirth, command.DateRegistered, command.Gender);

            try
            {
                petRepository.Update(pet);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return pet;
        }
        
    }