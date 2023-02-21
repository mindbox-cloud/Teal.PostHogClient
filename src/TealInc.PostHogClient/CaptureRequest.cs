namespace TealInc.PostHogClient;

public sealed record CaptureRequest(object DistinctId, string Event)
{
    public Dictionary<string, object> Properties { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}