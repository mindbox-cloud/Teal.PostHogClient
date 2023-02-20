namespace TealInc.PostHogClient;

public sealed class PostHogOptions
{
    public string ApiKey { get; set; } = default!;
    public string Host { get; set; } = "https://app.posthog.com/";
    public TimeSpan FlushInterval { get; set; } = TimeSpan.FromSeconds(10);
}