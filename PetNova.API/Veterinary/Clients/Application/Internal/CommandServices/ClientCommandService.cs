using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Clients.Domain.Repositories;
using PetNova.API.Veterinary.Clients.Domain.Services;

namespace PetNova.API.Veterinary.Client.Application.Internal.Services;
using ClientEntity = PetNova.API.Veterinary.Clients.Domain.Model.Aggregate.Client;

public class ClientCommandService : IClientCommandService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientCommandService(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientEntity> CreateAsync(ClientEntity client)
    {
        await _clientRepository.AddAsync(client);
        await _unitOfWork.CompleteAsync(); // ✅ Esto guarda el cliente en la BD
        return client;
    }

    // También agrega esta línea en los otros métodos que modifican datos
    public async Task<ClientEntity?> UpdateAsync(Guid clientId, ClientEntity updatedClient)
    {
        var existingClient = await _clientRepository.FindByIdAsync(clientId);
        if (existingClient == null) return null;

        existingClient.Update(updatedClient.Name, updatedClient.Phone);
        _clientRepository.Update(existingClient);
        await _unitOfWork.CompleteAsync(); 

        return existingClient;
    }

    public async Task<bool> DeleteAsync(Guid clientId)
    {
        var client = await _clientRepository.FindByIdAsync(clientId);
        if (client == null) return false;

        _clientRepository.Remove(client);
        await _unitOfWork.CompleteAsync(); 

        return true;
    }
}
