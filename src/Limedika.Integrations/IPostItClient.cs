namespace Limedika.Integrations;

public interface IPostItClient
{
    public Task<string?> GetPostCode(string address);
}
