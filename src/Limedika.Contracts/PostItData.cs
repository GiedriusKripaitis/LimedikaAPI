using Newtonsoft.Json;

namespace Limedika.Contracts;

public record PostItData
{
    [JsonProperty(PropertyName = "post_code")]
    public required string PostCode { get; init; }
    public required string Address { get; init; }
    public required string Street { get; init; }
    public required string Number { get; init; }
    [JsonProperty(PropertyName = "only_number")]
    public required string OnlyNumber { get; init; }
    public required string Housing { get; init; }
    public required string City { get; init; }
    public required string Eldership { get; init; }
    public required string Municipality { get; init; }
    public required string Post { get; init; }
    public required string Mailbox { get; init; }
}
