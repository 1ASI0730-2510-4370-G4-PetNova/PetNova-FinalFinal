using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Queries;
using PetNova.API.Veterinary.Pets.Domain.Repositories;
using PetNova.API.Veterinary.Pets.Domain.Services;

namespace PetNova.API.Veterinary.Pets.Application.Internal.QueryServices;


public class PetQueryService(IPetRepository petRepository) : IPetDomainQueryService
{
    public async Task<IEnumerable<Pet>> Handle(GetAllPetsQuery query)
    {
        return await petRepository.ListAsync();
    }

    public async Task<IEnumerable<Pet>> Handle(GetAllPetsByClientIdQuery query)
    {
        return await petRepository.FindByClientIdAsync(query.ClientId);
    }

    public async Task<Pet?> Handle(GetPetByIdQuery query)
    {
        return await petRepository.FindByIdAsync(query.PetId);
    }
}