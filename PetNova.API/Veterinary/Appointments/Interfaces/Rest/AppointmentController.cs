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
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentCommandService _commandService;
    private readonly IAppointmentQueryService _queryService;

    public AppointmentsController(IAppointmentCommandService commandService, IAppointmentQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<IEnumerable<AppointmentResource>> GetAll()
    {
        var result = await _queryService.HandleAsync(new GetAllAppointmentsQuery());
        return result.Select(AppointmentResourceAssembler.ToResource);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentResource>> GetById(Guid id)
    {
        var appointment = await _queryService.HandleAsync(new GetAppointmentByIdQuery(id));
        return AppointmentResourceAssembler.ToResource(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentResource>> Create(CreateAppointmentCommand command)
    {
        var appointment = await _commandService.HandleAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, AppointmentResourceAssembler.ToResource(appointment));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateAppointmentCommand command)
    {
        await _commandService.HandleAsync(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _commandService.HandleAsync(new DeleteAppointmentCommand(id));
        return NoContent();
    }
}
