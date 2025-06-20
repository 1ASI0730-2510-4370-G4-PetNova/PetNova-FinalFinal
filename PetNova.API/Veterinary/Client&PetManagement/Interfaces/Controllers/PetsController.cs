using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.ClientAndPetManagement.Application.Services;
using PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

namespace PetNova.API.Veterinary.Client_PetManagement.Interfaces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly PetService _petService;

    public PetsController(PetService petService)
    {
        _petService = petService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _petService.ListAsync();
        return Ok(pets);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pet = await _petService.GetByIdAsync(id);
        if (pet == null) return NotFound();
        return Ok(pet);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        var pets = await _petService.GetByClientIdAsync(clientId);
        return Ok(pets);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PetDTO petDto)
    {
        var pet = await _petService.CreateAsync(petDto);
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, PetDTO petDto)
    {
        var pet = await _petService.UpdateAsync(id, petDto);
        if (pet == null) return NotFound();
        return Ok(pet);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _petService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}