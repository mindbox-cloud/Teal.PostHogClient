using System.Text.Json.Serialization;

namespace PostHog.Internal;

internal sealed class PostHogEventBatch
{
    public PostHogEventBatch(string apiKey, ICollection<PostHogEvent> batch)
    {
        ApiKey = apiKey;
        Batch = batch;
    }

    [JsonPropertyName("api_key")]
    public string ApiKey { get; }
    public ICollection<PostHogEvent> Batch { get; }
}