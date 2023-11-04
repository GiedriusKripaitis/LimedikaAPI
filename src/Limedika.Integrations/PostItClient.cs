using Limedika.Contracts;
using Limedika.Contracts.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Limedika.Integrations;

public class PostItClient : IPostItClient
{
    private readonly ILogger<PostItClient> _logger;

    public PostItClient(ILogger<PostItClient> logger)
    {
        _logger = logger;
    }

    public async Task<string?> GetPostCode(string address)
    {
        try
        {
            HttpClient httpClient = new()
            {
                BaseAddress = new Uri(PostItSettings.Default.BaseAddress!),
            };

            string postItFriendlyAddress = address.Trim().Replace(" ", "+");

            using HttpResponseMessage response = await httpClient.GetAsync($"?term={postItFriendlyAddress}&key={PostItSettings.Default.Key!}");

            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            PostItResponse? postItResponse = JsonConvert.DeserializeObject<PostItResponse>(jsonResponse);

            if (postItResponse is null || !postItResponse.Data.Any())
            {
                _logger.LogWarning($"Post code for {address} was not found");
                // Could add this to the Log table too if we want to
                // but it looks more like a table for changes that HAPPENED rather than errors which resulted in things not happening

                return null;
            }

            if (postItResponse.Data.Count > 1)
            {
                _logger.LogWarning($"Found more than one post code for {address}");
                // Unless PM says "Just use the first one"

                return null;
            }

            return postItResponse.Data.First().PostCode;
        }
        catch (Exception e)
        {
            _logger.LogError("PostItClient failed to get post code. Exception: {Exception}", e);

            return null;
        }
    }
}
