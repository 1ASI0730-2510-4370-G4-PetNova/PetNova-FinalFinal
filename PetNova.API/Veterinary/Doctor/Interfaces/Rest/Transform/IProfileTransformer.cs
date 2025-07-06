namespace PetNova.API.Veterinary.Doctor.Interfaces.Rest.Transform;
using PetNova.API.Veterinary.Doctor.Interfaces.Rest.Resources;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
public interface IProfileTransformer
{
    IProfileResource ToResource(DoctorProfileAggregate aggregate);
    DoctorProfileAggregate ToAggregate(IProfileResource resource);
}