using AutoMapper;
using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Domain.Model.Queries;
using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Veterinary.Appointments.Domain.Services;

namespace PetNova.API.Veterinary.Appointments.Application.Internal.QueryServices;

public class AppointmentQueryService : IAppointmentQueryService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentQueryService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Appointment>> HandleAsync(GetAllAppointmentsQuery query)
        => _repository.GetAllAsync(query.Page, query.PageSize);

    public Task<Appointment> HandleAsync(GetAppointmentByIdQuery query)
        => _repository.GetByIdAsync(query.Id) ?? throw new KeyNotFoundException();

    public Task<IEnumerable<Appointment>> HandleAsync(GetAppointmentsByStatusQuery query)
        => _repository.GetByStatusAsync(query.Status);
}
