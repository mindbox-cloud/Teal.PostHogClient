using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace PostHog.Internal;

public interface IPostHogApi
{
    Task BatchAsync(PostHogEventBatch batch);
}

public sealed class PostHogApi : IPostHogApi
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PostHogApi> _logger;
    
    private readonly Uri _batchUri;

    public PostHogApi(PostHogOptions options, ILogger<PostHogApi> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        var baseUri = new Uri(options.Host);
        _batchUri = new Uri(baseUri, "/batch");
    }

    public async Task BatchAsync(PostHogEventBatch batch)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync(_batchUri, batch);
        responseMessage.EnsureSuccessStatusCode();
        
        _logger.LogDebug("Sent batch of {Count} events", batch.Batch.Count);
    }
}