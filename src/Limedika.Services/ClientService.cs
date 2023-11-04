using Limedika.Contracts;
using Limedika.Infrastructure.Models;
using Limedika.Infrastructure.Repositories;
using Limedika.Integrations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Limedika.Services;

public class ClientService : IClientService
{
    private readonly ILogger<ClientService> _logger;
    private readonly IClientsRepository _clientsRepository;
    private readonly IPostItClient _postItClient;

    public ClientService(ILogger<ClientService> logger, IClientsRepository clientsRepository, IPostItClient postItClient)
    {
        _logger = logger;
        _clientsRepository = clientsRepository;
        _postItClient = postItClient;
    }

    public async Task ImportClients()
    {
        List<Client>? clients;

        try
        {
            string clientsJson = await File.ReadAllTextAsync("Data/clients.json");
            clients = JsonConvert.DeserializeObject<List<Client>>(clientsJson);
        }
        catch
        {
            _logger.LogError("Failed to read clients from the data file");
            throw;
        }

        if (clients is not null && clients.Any())
        {
            List<ClientEntity> existingClientEntities = await _clientsRepository.GetClients();

            List<ClientEntity> newClientEntities = new();

            foreach (Client client in clients)
            {
                if (!existingClientEntities.Exists(x => x.Name == client.Name))
                {
                    newClientEntities.Add(new ClientEntity
                    {
                        Name = client.Name,
                        Address = client.Address,
                        PostCode = client.PostCode
                    });
                }
            }

            await _clientsRepository.InsertClients(newClientEntities);
        }
    }

    public async Task<List<Client>> GetClients()
    {
        List<ClientEntity> clientEntities = await _clientsRepository.GetClients();

        List<Client> clients = clientEntities.Select(x => new Client
        {
            Id = x.Id,
            Name = x.Name,
            Address = x.Address,
            PostCode = x.PostCode,
            CreatedOn = x.CreatedOn
        }).ToList();

        return clients;
    }

    public async Task UpdatePostCodes()
    {
        // I'd make this only fetch clients without post codes, but hey, you want to update, not to fill in gaps
        List<ClientEntity> existingClientEntities = await _clientsRepository.GetClients();

        foreach (ClientEntity client in existingClientEntities)
        {
            string? postCode = await _postItClient.GetPostCode(client.Address);

            if (postCode is not null )
            {
                await _clientsRepository.UpdatePostCodes(client.Id, postCode);
            }
        }
    }
}
