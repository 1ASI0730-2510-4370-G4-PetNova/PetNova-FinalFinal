using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Status.Application.Services;
using PetNova.API.Veterinary.Status.Interface.DTOs;

namespace PetNova.API.Veterinary.Status.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly StatusService _statusService;

    public StatusController(StatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var statuses = await _statusService.ListAsync();
        return Ok(statuses);
    }

    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var statuses = await _statusService.ListByTypeAsync(type);
        return Ok(statuses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var status = await _statusService.GetByIdAsync(id);
        if (status == null) return NotFound();
        return Ok(status);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StatusDTO statusDto)
    {
        var status = await _statusService.CreateAsync(statusDto);
        return CreatedAtAction(nameof(GetById), new { id = status.Id }, status);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, StatusDTO statusDto)
    {
        var status = await _statusService.UpdateAsync(id, statusDto);
        if (status == null) return NotFound();
        return Ok(status);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _statusService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}