using Newtonsoft.Json;

namespace Limedika.Contracts;

public record PostItResponse
{
    public required string Status { get; init; }
    public bool Success { get; init; }
    public required string Message { get; init; }
    [JsonProperty(PropertyName = "message_code")]
    public int MessageCode { get; init; }
    public int Total { get; init; }
    public List<PostItData> Data { get; init; } = new();
}
