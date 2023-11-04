namespace Limedika.Contracts.Settings;

public class PostItSettings
{
    public string? BaseAddress { get; set; }
    public string? Key { get; set; }

    public static PostItSettings Default { get; set; } = new();
}
