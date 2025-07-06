using Microsoft.AspNetCore.Mvc;
using PetNova.API.Veterinary.Clients.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Clients.Domain.Services;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Transform;
using ClientEntity = PetNova.API.Veterinary.Clients.Domain.Model.Aggregate.Client;

namespace PetNova.API.Veterinary.Clients.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientCommandService _commandService;
    private readonly IClientQueryService _queryService;

    public ClientsController(IClientCommandService commandService, IClientQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterClient([FromBody] CreateClientResource resource)
    {
        var command = CreateClientCommandFromResourceAssembler.ToCommandFromResource(resource);
        var fullName = new Domain.Model.ValueObjects.FullName(command.FirstName, command.LastName);
        var client = new ClientEntity(fullName, command.Email, command.Phone);

        var result = await _commandService.CreateAsync(client);
        var resourceResult = ClientResourceFromEntityAssembler.ToResourceFromEntity(result);

        return CreatedAtAction(nameof(GetClientById), new { id = result.Id }, resourceResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(Guid id)
    {
        var client = await _queryService.GetByIdAsync(id);
        if (client == null) return NotFound();

        var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
        return Ok(resource);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var clients = await _queryService.ListAsync();
        var resources = clients.Select(ClientResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientResource resource)
    {
        var client = await _queryService.GetByIdAsync(id);
        if (client == null) return NotFound();

        var currentName = client.Name;
        var updatedName = new FullName(
            resource.FirstName ?? currentName.FirstName,
            resource.LastName ?? currentName.LastName);
    
        var updatedPhone = resource.Phone ?? client.Phone;

        var updatedClient = new ClientEntity(updatedName, client.Email, updatedPhone);

        var result = await _commandService.UpdateAsync(id, updatedClient);
        if (result == null) return NotFound();

        return Ok(ClientResourceFromEntityAssembler.ToResourceFromEntity(result));
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var deleted = await _commandService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
