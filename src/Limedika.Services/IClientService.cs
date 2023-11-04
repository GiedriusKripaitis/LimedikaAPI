using Limedika.Contracts;

namespace Limedika.Services;

public interface IClientService
{
    public Task ImportClients();
    public Task<List<Client>> GetClients();
    public Task UpdatePostCodes();
}
