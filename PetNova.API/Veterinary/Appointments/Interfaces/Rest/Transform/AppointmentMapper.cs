using AutoMapper;
using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;

public class AppointmentMapper : Profile
{
    public AppointmentMapper()
    {
        CreateMap<Appointment, AppointmentResource>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration)) // sin Parse
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<AppointmentResource, Appointment>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration)) // sin Parse
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<AppointmentStatus>(src.Status)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<AppointmentType>(src.Type)));
    }
}