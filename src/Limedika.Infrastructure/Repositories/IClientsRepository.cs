using Limedika.Infrastructure.Models;

namespace Limedika.Infrastructure.Repositories;

public interface IClientsRepository
{

    public Task InsertClients(List<ClientEntity> clients);
    public Task<List<ClientEntity>> GetClients();
    public Task UpdatePostCodes(int id, string postCode);
}
