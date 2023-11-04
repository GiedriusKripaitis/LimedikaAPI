namespace Limedika.Infrastructure.Models;

public record LogEntryEntity : EntityBase
{
    public LogEntryType Type { get; init; }
    public required string Entry { get; init; }
    public DateTime CreatedOn { get; init; }
}
