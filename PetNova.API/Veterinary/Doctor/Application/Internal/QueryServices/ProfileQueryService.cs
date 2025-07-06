using PetNova.API.Veterinary.Doctor.Domain.Model.Queries;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;

namespace PetNova.API.Veterinary.Doctor.Application.Internal.QueryServices;

public class ProfileQueryService : IProfileQueryService
{
    private readonly IProfileRepository _repository;

    public ProfileQueryService(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProfileResponse> Execute(GetProfileQuery query)
    {
        var aggregate = await _repository.GetByIdAsync(query.Id);
        return aggregate.ToResponse();
    }

    public async Task<ProfilePictureResponse> Execute(GetProfilePictureQuery query)
    {
        var aggregate = await _repository.GetByIdAsync(query.Id);
        return new ProfilePictureResponse(aggregate.ProfilePictureUrl);
    }
}