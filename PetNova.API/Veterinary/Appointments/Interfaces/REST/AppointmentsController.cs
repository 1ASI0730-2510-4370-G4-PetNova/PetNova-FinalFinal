using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Appointments.Domain.Model.Queries;
using PetNova.API.Veterinary.Appointments.Domain.Services;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Appointments.Interfaces.REST.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetNova.API.Shared.Interfaces.ASP; // For BaseController
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects; // Added for AppointmentStatus

namespace PetNova.API.Veterinary.Appointments.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AppointmentsController : BaseController // Inheriting from BaseController
    {
        private readonly IAppointmentCommandService _appointmentCommandService;
        private readonly IAppointmentQueryService _appointmentQueryService;

        public AppointmentsController(IAppointmentCommandService appointmentCommandService, IAppointmentQueryService appointmentQueryService)
        {
            _appointmentCommandService = appointmentCommandService;
            _appointmentQueryService = appointmentQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = CreateAppointmentCommandFromResourceAssembler.ToCommandFromResource(resource);
            try
            {
                var appointment = await _appointmentCommandService.HandleAsync(command);
                var appointmentResource = AppointmentResourceFromEntityAssembler.ToResourceFromEntity(appointment);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentResource.Id }, appointmentResource);
            }
            catch (Exception ex) // Consider more specific exception handling
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var query = new GetAppointmentByIdQuery(id);
            var appointment = await _appointmentQueryService.HandleAsync(query);
            if (appointment == null)
                return NotFound();

            var resource = AppointmentResourceFromEntityAssembler.ToResourceFromEntity(appointment);
            return Ok(resource);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null)
        {
            var query = new GetAllAppointmentsQuery(page, pageSize, sortBy);
            var appointments = await _appointmentQueryService.HandleAsync(query);
            var resources = appointments.Select(AppointmentResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetAppointmentsByStatus([FromQuery] string status, [FromQuery] DateTime? fromDate = null)
        {
            // Assuming AppointmentStatus can be parsed from string.
            if (!System.Enum.TryParse<AppointmentStatus>(status, true, out var parsedStatus)) // Use AppointmentStatus directly
            {
                return BadRequest("Invalid status value.");
            }
            var query = new GetAppointmentsByStatusQuery(parsedStatus, fromDate);
            var appointments = await _appointmentQueryService.HandleAsync(query);
            var resources = appointments.Select(AppointmentResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = UpdateAppointmentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            try
            {
                await _appointmentCommandService.HandleAsync(command);
                return NoContent();
            }
            catch (Exception ex) // Consider more specific exception handling (e.g., NotFoundException)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var command = new Domain.Model.Commands.DeleteAppointmentCommand(id);
            try
            {
                await _appointmentCommandService.HandleAsync(command);
                return NoContent();
            }
            catch (Exception ex) // Consider more specific exception handling (e.g., NotFoundException)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
