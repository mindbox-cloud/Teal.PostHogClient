namespace PostHog;

public sealed record CaptureRequest(string DistinctId, string Event)
{
    public Dictionary<string, object> Properties { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}