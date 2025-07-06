using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Model.Queries;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Appointments.Domain.Services;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Resources;
using PetNova.API.Veterinary.Appointments.Interfaces.Rest.Transform;
namespace PetNova.API.Veterinary.Appointments.Interfaces.Rest;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentCommandService _commandService;
    private readonly IAppointmentQueryService _queryService;
    private readonly IMapper _mapper;

    public AppointmentController(
        IAppointmentCommandService commandService,
        IAppointmentQueryService queryService,
        IMapper mapper)
    {
        _commandService = commandService;
        _queryService = queryService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllAppointmentsQuery { Page = page, PageSize = pageSize };
        var appointments = await _queryService.HandleAsync(query);
        return Ok(_mapper.Map<List<AppointmentResource>>(appointments));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetAppointmentByIdQuery(id);
        var appointment = await _queryService.HandleAsync(query);
        return Ok(_mapper.Map<AppointmentResource>(appointment));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentResource resource)
    {
        var command = _mapper.Map<CreateAppointmentCommand>(resource);
        var appointment = await _commandService.HandleAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, _mapper.Map<AppointmentResource>(appointment));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentResource resource)
    {
        // Crear el command manualmente para evitar problemas con propiedades init-only
        var command = new UpdateAppointmentCommand(
            Id: id,
            NewStartDate: resource.StartDate,
            NewStatus: resource.Status != null ? 
                Enum.Parse<AppointmentStatus>(resource.Status) : null
        );
        
        await _commandService.HandleAsync(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteAppointmentCommand(id);
        await _commandService.HandleAsync(command);
        return NoContent();
    }
}