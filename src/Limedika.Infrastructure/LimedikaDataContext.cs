using Limedika.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Limedika.Infrastructure;

public class LimedikaDataContext : DbContext
{
    public LimedikaDataContext(DbContextOptions<LimedikaDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Limedika");
        base.OnModelCreating(modelBuilder);

        //Default values
        modelBuilder.Entity<ClientEntity>().Property(x => x.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<LogEntryEntity>().Property(x => x.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
    }

    public DbSet<ClientEntity> Clients { get; set; } = default!;
    public DbSet<LogEntryEntity> LogEntries { get; set; } = default!;
}
