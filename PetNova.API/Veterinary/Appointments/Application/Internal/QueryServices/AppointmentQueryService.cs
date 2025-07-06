using AutoMapper;
using PetNova.API.Veterinary.Appointments.Domain.Model.Queries;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Veterinary.Appointments.Domain.Services;

namespace PetNova.API.Veterinary.Appointments.Application.Internal.QueryServices;

internal sealed class AppointmentQueryService : IAppointmentQueryService
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;

    public AppointmentQueryService(
        IAppointmentRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // Obtener todas las citas (con paginación/filtros)
    public async Task<IEnumerable<Appointment>> HandleAsync(GetAllAppointmentsQuery query)
    {
        return await _repository.GetAllAsync(
            page: query.Page,
            pageSize: query.PageSize,
            sortBy: query.SortBy
        );
    }

    // Obtener cita por ID
    public async Task<Appointment> HandleAsync(GetAppointmentByIdQuery query)
    {
        return await _repository.GetByIdAsync(query.Id) 
               ?? throw new AppointmentNotFoundException(query.Id);
    }

    // Obtener citas por estado (y fecha opcional)
    public async Task<IEnumerable<Appointment>> HandleAsync(GetAppointmentsByStatusQuery query)
    {
        return await _repository.GetByStatusAsync(
            status: query.Status,
            fromDate: query.FromDate
        );
    }
}

internal class AppointmentNotFoundException : Exception
{
    public AppointmentNotFoundException(Guid appointmentId)
        : base($"Appointment with ID '{appointmentId}' not found.")
    {
    }
}