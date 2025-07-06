using AutoMapper;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest.Transform;

public class AppointmentMapper : Profile
{
    public AppointmentMapper()
    {
        // Entity -> Resource
        CreateMap<Appointment, AppointmentResource>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        
        // CreateResource -> Command
        CreateMap<CreateAppointmentResource, CreateAppointmentCommand>();
        
        // UpdateResource -> Command
        CreateMap<UpdateAppointmentResource, UpdateAppointmentCommand>();
    }
}