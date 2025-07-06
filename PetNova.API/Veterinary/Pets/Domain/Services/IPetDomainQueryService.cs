using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Queries;

namespace PetNova.API.Veterinary.Pets.Domain.Services;

/// <summary>
///     Interface for Pet query service.
/// </summary>
/// <remarks>
///     This interface defines the operations related to querying pets.
/// </remarks>
/// 
public interface IPetDomainQueryService
{
    Task<IEnumerable<Pet>> Handle(GetAllPetsQuery query);
    Task<IEnumerable<Pet>> Handle(GetAllPetsByClientIdQuery query);
    Task<Pet?> Handle(GetPetByIdQuery query);
}