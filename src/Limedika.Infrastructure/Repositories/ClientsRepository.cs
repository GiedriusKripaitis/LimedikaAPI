using Limedika.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Limedika.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly LimedikaDataContext _context;

    public ClientsRepository(LimedikaDataContext context)
    {
        _context = context;
    }

    public async Task InsertClients(List<ClientEntity> clients)
    {
        await _context.Clients.AddRangeAsync(clients);
        await _context.SaveChangesAsync();

        // Yes, I don't like having it here either, but this way it's automated and cannot be meddled with
        await LogInsertedClients(clients);
    }

    public async Task<List<ClientEntity>> GetClients()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task UpdatePostCodes(int id, string postCode)
    {
        await _context.Clients.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.PostCode, postCode));

        await LogUpdatedPostCode(id, postCode);
    }

    private async Task LogInsertedClients(IEnumerable<ClientEntity> clients)
    {
        List<LogEntryEntity> logEntries = clients.Select(client => new LogEntryEntity 
            {
                Type = LogEntryType.InsertedClient,
                Entry = $"Inserted client with Id {client.Id} and name \"{client.Name}\""
            })
            .ToList();

        await _context.LogEntries.AddRangeAsync(logEntries);
        await _context.SaveChangesAsync();
    }

    private async Task LogUpdatedPostCode(int id, string postCode)
    {
        LogEntryEntity logEntry = new()
        {
            Type = LogEntryType.UpdatedPostCode,
            Entry = $"Updated post code of client with Id {id} to {postCode}"
        };

        await _context.LogEntries.AddAsync(logEntry);
        await _context.SaveChangesAsync();
    }
}
