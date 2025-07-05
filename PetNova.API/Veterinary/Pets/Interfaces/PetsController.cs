using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Pets.Domain.Model.Commands;
using PetNova.API.Veterinary.Pets.Domain.Model.Queries;
using PetNova.API.Veterinary.Pets.Domain.Services;
using PetNova.API.Veterinary.Pets.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Pets.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace PetNova.API.Veterinary.Pets.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Pets")]
public class PetController(
    IPetDomainQueryService petQueryService,
    IPetDomainCommandService petCommandService
) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new pet", OperationId = "post_api_Pets")]
    [SwaggerResponse(201, "The pet was created", typeof(PetResource))]
    [SwaggerResponse(400, "The pet could not be created")]
    public async Task<IActionResult> Create([FromBody] CreatePetResource resource)
    {
        var command = CreatePetCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await petCommandService.Handle(command);
        if (result is null) return BadRequest();
        var petResource = PetResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, petResource);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "List all pets", OperationId = "get_api_Pets")]
    [SwaggerResponse(200, "Returns all pets", typeof(IEnumerable<PetResource>))]
    public async Task<IActionResult> GetAll()
    {
        var pets = await petQueryService.Handle(new GetAllPetsQuery());
        var resources = pets.Select(PetResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get pet by ID", OperationId = "get_api_Pets__id_")]
    [SwaggerResponse(200, "Returns the pet", typeof(PetResource))]
    [SwaggerResponse(404, "Pet not found")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPetByIdQuery(id);
        var pet = await petQueryService.Handle(query);
        if (pet is null) return NotFound();
        return Ok(PetResourceFromEntityAssembler.ToResourceFromEntity(pet));
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update a pet", OperationId = "put_api_Pets__id_")]
    [SwaggerResponse(200, "Pet updated", typeof(PetResource))]
    [SwaggerResponse(404, "Pet not found")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreatePetResource resource)
    {
        var command = CreatePetCommandFromResourceAssembler.ToCommandFromResource(resource);
        var updatedPet = await petCommandService.Handle(id, command);
        if (updatedPet is null) return NotFound();
        return Ok(PetResourceFromEntityAssembler.ToResourceFromEntity(updatedPet));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete a pet", OperationId = "delete_api_Pets__id_")]
    [SwaggerResponse(204, "Pet deleted")]
    [SwaggerResponse(404, "Pet not found")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await petCommandService.Handle(new DeletePetCommand(id));
        return success ? NoContent() : NotFound();
    }

    [HttpGet("client/{id:guid}")]
    [SwaggerOperation(Summary = "Get pets by client ID", OperationId = "get_api_Pets_client__id_")]
    [SwaggerResponse(200, "Returns pets belonging to client", typeof(IEnumerable<PetResource>))]
    public async Task<IActionResult> GetByClientId(Guid id)
    {
        var pets = await petQueryService.Handle(new GetAllPetsByClientIdQuery(id));
        var resources = pets.Select(PetResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
