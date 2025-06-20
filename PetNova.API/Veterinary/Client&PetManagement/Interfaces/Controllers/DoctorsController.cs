using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.Client_PetManagement.Interfaces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _doctorService;

    public DoctorsController(DoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _doctorService.ListAsync();
        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor == null) return NotFound();
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DoctorDTO doctorDto)
    {
        var doctor = await _doctorService.CreateAsync(doctorDto);
        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, DoctorDTO doctorDto)
    {
        var doctor = await _doctorService.UpdateAsync(id, doctorDto);
        if (doctor == null) return NotFound();
        return Ok(doctor);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _doctorService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}