using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Doctor.Application.Internal.CommandServices;
using PetNova.API.Veterinary.Doctor.Application.Internal.QueryServices;
using PetNova.API.Veterinary.Doctor.Domain.Model.Commands;
using PetNova.API.Veterinary.Doctor.Domain.Model.Queries;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;

namespace PetNova.API.Veterinary.Doctor.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/doctors")]
[Produces("application/json")]
public class DoctorProfileController : ControllerBase
{
    private readonly IProfileCommandService _commandService;
    private readonly IProfileQueryService _queryService;
    private readonly ILogger<DoctorProfileController> _logger;

    public DoctorProfileController(
        IProfileCommandService commandService,
        IProfileQueryService queryService,
        ILogger<DoctorProfileController> logger)
    {
        _commandService = commandService;
        _queryService = queryService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> GetProfile(Guid id)
    {
        var query = new GetProfileQuery(id);
        var result = await _queryService.Execute(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet("{id}/picture")]
    [ProducesResponseType(typeof(ProfilePictureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfilePictureResponse>> GetProfilePicture(Guid id)
    {
        var query = new GetProfilePictureQuery(id);
        var result = await _queryService.Execute(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfileResponse>> CreateProfile([FromBody] UpdateProfileCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var aggregate = command.ToAggregate();
        await _commandService.AddProfileAsync(aggregate);
        
        var response = new ProfileResponse(
            aggregate.Id,
            aggregate.Name.ToString(),
            aggregate.Specialty.Value,
            aggregate.Biography.Content,
            aggregate.ProfilePictureUrl);

        return CreatedAtAction(
            actionName: nameof(GetProfile),
            routeValues: new { id = aggregate.Id },
            value: response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateProfileCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Verificar si el perfil existe primero
        var existingProfile = await _queryService.Execute(new GetProfileQuery(id));
        if (existingProfile == null)
        {
            return NotFound();
        }

        try
        {
            var aggregate = command.ToAggregate();
            await _commandService.UpdateProfileAsync(aggregate);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating doctor profile {ProfileId}", id);
            return Conflict("El registro ha sido modificado o eliminado por otro usuario. Por favor refresque los datos e intente nuevamente.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Doctor profile not found during update {ProfileId}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor profile {ProfileId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al actualizar el perfil");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfile(Guid id)
    {
        try
        {
            // Verificar si existe antes de eliminar
            var existingProfile = await _queryService.Execute(new GetProfileQuery(id));
            if (existingProfile == null)
            {
                return NotFound();
            }

            await _commandService.DeleteProfileAsync(id);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict deleting doctor profile {ProfileId}", id);
            return Conflict("El registro ha sido modificado o eliminado por otro usuario.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor profile {ProfileId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al eliminar el perfil");
        }
    }
}