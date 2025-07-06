using Microsoft.AspNetCore.Mvc;
using PetNova.API.Shared.Interfaces.ASP; // For BaseController
using PetNova.API.Veterinary.Clients.Domain.Services;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;
using PetNova.API.Veterinary.Clients.Interfaces.REST.Transform;
using System;
using System.Linq;
using System.Threading.Tasks;
using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Clients.Domain.Model.Commands;
using PetNova.API.Veterinary.Clients.Domain.Model.ValueObjects; // For FullName
// using PetNova.API.Veterinary.Clients.Domain.Model.Queries; // Not directly used here but good for context

namespace PetNova.API.Veterinary.Clients.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientsController : BaseController
    {
        private readonly IClientCommandService _clientCommandService;
        private readonly IClientQueryService _clientQueryService;

        public ClientsController(IClientCommandService clientCommandService, IClientQueryService clientQueryService)
        {
            _clientCommandService = clientCommandService;
            _clientQueryService = clientQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = CreateClientCommandFromResourceAssembler.ToCommandFromResource(resource);

            var clientName = new FullName(command.FirstName, command.LastName);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            // Using the Client constructor that takes all necessary fields
            var clientToCreate = new Client(
                            name: clientName,
                            email: command.Email,
                            phone: command.Phone,
                            address: command.Address,
                            passwordHash: passwordHash
                            );

            try
            {
                var createdClient = await _clientCommandService.CreateAsync(clientToCreate);
                if (createdClient == null) return BadRequest("Could not create client.");

                var clientResource = ClientResourceFromEntityAssembler.ToResourceFromEntity(createdClient);
                return CreatedAtAction(nameof(GetClientById), new { id = clientResource.Id }, clientResource);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return BadRequest($"An error occurred while creating the client: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await _clientQueryService.GetByIdAsync(id);
            if (client == null)
                return NotFound();

            var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
            return Ok(resource);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientQueryService.ListAsync();
            var resources = clients.Select(ClientResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clientToUpdate = await _clientQueryService.GetByIdAsync(id);
            if (clientToUpdate == null) return NotFound();

            // Construct FullName for update if names are provided
            var currentName = clientToUpdate.Name;
            var newFirstName = resource.FirstName ?? currentName.FirstName;
            var newLastName = resource.LastName ?? currentName.LastName;
            var newFullName = new FullName(newFirstName, newLastName);

            // The UpdateClientCommand is assembled but not directly used by the service,
            // which expects a Client entity. This is a point of potential refactoring for stricter CQRS.
            var command = UpdateClientCommandFromResourceAssembler.ToCommandFromResource(id, resource);

            // Update client fields using methods on the aggregate
            clientToUpdate.Update(
                newFullName,
                command.Phone ?? clientToUpdate.Phone, // Use command properties
                command.Email ?? clientToUpdate.Email,
                command.Address ?? clientToUpdate.Address
            );
            // Password update would typically be a separate, more secure endpoint/process
            // e.g., clientToUpdate.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(newPassword));


            try
            {
                var updatedClient = await _clientCommandService.UpdateAsync(id, clientToUpdate);
                if (updatedClient == null)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                 // Log the exception (ex)
                return BadRequest($"An error occurred while updating the client: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            try
            {
                var success = await _clientCommandService.DeleteAsync(id);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return BadRequest($"An error occurred while deleting the client: {ex.Message}");
            }
        }
    }
}
