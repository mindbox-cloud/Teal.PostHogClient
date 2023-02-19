using PostHog.Internal;

namespace PostHog;

public sealed class PostHogOptions
{
    public string ApiKey { get; set; } = default!;
    public string Host { get; set; } = "https://app.posthog.com/";
    public TimeSpan FlushInterval { get; set; } = TimeSpan.FromSeconds(10);
    public bool Debug { get; set; }
    public Func<HttpClient> HttpClientFactory { get; set; } = () => Default.HttpClient;
}