namespace Limedika.Api.Host.Capabilities;

public class SwaggerConfig
{
    public bool Operational { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? RouteTemplate { get; set; }
    public string? SwaggerEndpoint { get; set; }
    public string? RoutePrefix { get; set; }

    public static SwaggerConfig Default { get; set; } = new();
}
