using Microsoft.Extensions.Logging;
using TealInc.PostHogClient.Internal;

namespace TealInc.PostHogClient;

public sealed class PostHogClient : IPostHogClient, IAsyncDisposable
{
    private readonly IQueueManager _queueManager;
    private readonly ILogger<PostHogClient> _logger;

    public PostHogClient(IQueueManager queueManager, ILogger<PostHogClient> logger)
    {
        _queueManager = queueManager;
        _logger = logger;
        
        _logger.LogDebug("Started listening for events");
    }

    public async ValueTask DisposeAsync()
    {
        await _queueManager.StopAsync();

        _logger.LogDebug("Stopped listening for events");
    }

    public void Capture(CaptureRequest request)
    {
        var postHogEvent = new PostHogEvent
        {
            Event = request.Event,
            Properties = request.Properties,
            Timestamp = request.Timestamp.ToString("O")
        };
        postHogEvent.Properties.Add("distinct_id", request.DistinctId);

        _queueManager.Enqueue(postHogEvent);
    }
}