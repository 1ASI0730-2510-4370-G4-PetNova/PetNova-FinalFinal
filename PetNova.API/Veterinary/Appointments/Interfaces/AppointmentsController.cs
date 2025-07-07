using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Transform;
using PetNova.API.Veterinary.Appointments.Domain.Services;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Appointments.Domain.Model.Commands;
using PetNova.API.Veterinary.Appointments.Domain.Model.Query;
using Swashbuckle.AspNetCore.Annotations;

namespace PetNova.API.Veterinary.Appointments.Interfaces;

[ApiController]
[Route("api/Appointments")]
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
    {
        var appointment = await _commandService.Handle(command);
        var resource = AppointmentResourceAssembler.ToResource(appointment);
        return CreatedAtAction(nameof(GetAll), new { id = appointment.Id }, resource);
    }
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete an appointment", OperationId = "delete_api_Appointments__id_")]
    [SwaggerResponse(204, "Appointment deleted")]
    [SwaggerResponse(404, "Appointment not found")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _commandService.Handle(new DeleteAppointmentCommand(id));
        return result ? NoContent() : NotFound();
    }
    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update an appointment", OperationId = "put_api_Appointments__id_")]
    [SwaggerResponse(200, "Appointment updated", typeof(AppointmentResource))]
    [SwaggerResponse(404, "Appointment not found")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateAppointmentCommand command)
    {
        var result = await _commandService.Handle(id, command);
        if (result == null) return NotFound();

        var resource = AppointmentResourceAssembler.ToResource(result);
        return Ok(resource);
    }
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get appointment by ID", OperationId = "get_api_Appointments__id_")]
    [SwaggerResponse(200, "Appointment found", typeof(AppointmentResource))]
    [SwaggerResponse(404, "Appointment not found")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _queryService.HandleAsync(new GetAppointmentByIdQuery(id));
        if (result == null) return NotFound();

        var resource = AppointmentResourceAssembler.ToResource(result);
        return Ok(resource);
    }


}
