using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Appointments.Application.Services;
using PetNova.API.Veterinary.Appointments.Interfaces.DTOs;

namespace PetNova.API.Veterinary.Appointments.Interfaces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly AppointmentService _appointmentService;

    public AppointmentsController(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _appointmentService.ListAsync();
        return Ok(appointments);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null) return NotFound();
        return Ok(appointment);
    }

    [HttpGet("pet/{petId:guid}")]
    public async Task<IActionResult> GetByPetId(Guid petId)
    {
        var appointments = await _appointmentService.GetByPetIdAsync(petId);
        return Ok(appointments);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetByDoctorId(Guid doctorId)
    {
        var appointments = await _appointmentService.GetByDoctorIdAsync(doctorId);
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentDto appointmentDto)
    {
        var appointment = await _appointmentService.CreateAsync(appointmentDto);
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, AppointmentDto appointmentDto)
    {
        var appointment = await _appointmentService.UpdateAsync(id, appointmentDto);
        if (appointment == null) return NotFound();
        return Ok(appointment);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _appointmentService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}