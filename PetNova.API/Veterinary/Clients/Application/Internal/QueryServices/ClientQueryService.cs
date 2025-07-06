using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Clients.Domain.Repositories;
using PetNova.API.Veterinary.Clients.Domain.Services;

namespace PetNova.API.Veterinary.Client.Application.Internal.Services;
using ClientEntity = PetNova.API.Veterinary.Clients.Domain.Model.Aggregate.Client;

public class ClientQueryService : IClientQueryService
{
    private readonly IClientRepository _clientRepository;

    public ClientQueryService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientEntity?> GetByIdAsync(Guid clientId)
    {
        return await _clientRepository.FindByIdAsync(clientId);
    }


    public async Task<IEnumerable<ClientEntity>> ListAsync()
    {
        return await _clientRepository.ListAsync();
    }
}