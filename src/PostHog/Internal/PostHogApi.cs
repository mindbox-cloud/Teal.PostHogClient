using System.Net.Http.Json;

namespace PostHog.Internal;

internal interface IPostHogApi
{
    Task BatchAsync(PostHogEventBatch batch);
}

internal sealed class PostHogApi : IPostHogApi
{
    private readonly HttpClient _httpClient;
    private readonly IInternalLogger _internalLogger;

    public PostHogApi(PostHogOptions options, IInternalLogger internalLogger)
    {
        _internalLogger = internalLogger;
        _httpClient = options.HttpClientFactory();
        _httpClient.BaseAddress = new Uri(options.Host);
    }

    public async Task BatchAsync(PostHogEventBatch batch)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("/batch", batch);
        responseMessage.EnsureSuccessStatusCode();

        if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
        {
            _internalLogger.Log(InternalLogLevel.Debug, "Sent batch of {0} events", null, batch.Batch.Count);
        }
    }
}