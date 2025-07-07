using System.Collections.Generic;
using System.Threading.Tasks;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;
using PetNova.API.Veterinary.Appointments.Domain.Model.Query;
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

    public async Task<IEnumerable<Appointment>> HandleAsync(GetAllAppointmentsQuery query)
    {
        return await _repository.GetAllAsync();
    }
    public async Task<Appointment?> HandleAsync(GetAppointmentByIdQuery query)
    {
        return await _repository.GetByIdAsync(query.AppointmentId);
    }
}
