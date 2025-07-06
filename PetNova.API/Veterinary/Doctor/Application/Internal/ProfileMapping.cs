using PetNova.API.Veterinary.Doctor.Domain.Model.Commands;
using PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Model.Queries;

namespace PetNova.API.Veterinary.Doctor.Application.Internal;

internal static class ProfileMapping
{
    public static ProfileResponse ToResponse(this DoctorProfileAggregate aggregate)
    {
        return new ProfileResponse(
            aggregate.Id,
            aggregate.Name.ToString(),
            aggregate.Specialty.Value,
            aggregate.Biography.Content,
            aggregate.ProfilePictureUrl);
    }

    public static DoctorProfileAggregate ToAggregate(this UpdateProfileCommand command)
    {
        var name = new FullName(command.FirstName, command.LastName);
        var specialty = new Specialty(command.Specialty);
        var biography = new Biography(command.Biography);
        
        return DoctorProfileAggregate.Create(
            name,
            specialty,
            biography);
    }
}