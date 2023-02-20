using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace PostHog.Internal;

public interface IQueueManager
{
    void Enqueue(PostHogEvent postHogEvent);
    Task StopAsync();
}

public sealed class QueueManager : PeriodicTask, IQueueManager
{
    private readonly ConcurrentQueue<PostHogEvent> _queue = new();

    private readonly PostHogOptions _options;
    private readonly IPostHogApi _api;
    private readonly ILogger<QueueManager> _logger;

    public QueueManager(PostHogOptions options, IPostHogApi api, ILogger<QueueManager> logger) : base(options.FlushInterval)
    {
        _options = options;
        _api = api;
        _logger = logger;
    }

    public void Enqueue(PostHogEvent postHogEvent)
    {
        _queue.Enqueue(postHogEvent);

        _logger.LogDebug("Enqueued {Event} with ts {Timestamp}", postHogEvent.Event, postHogEvent.Timestamp);
    }

    protected override async Task OnTickAsync()
    {
        _logger.LogDebug("Flushing queue");

        var events = new List<PostHogEvent>();
        while (_queue.TryDequeue(out var postHogEvent))
        {
            events.Add(postHogEvent);
            
            _logger.LogDebug("Dequeued {Event} with ts {Timestamp}", postHogEvent.Event, postHogEvent.Timestamp);
        }

        if (!events.Any())
        {
            return;
        }

        var batch = new PostHogEventBatch(_options.ApiKey, events);

        try
        {
            await _api.BatchAsync(batch);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while sending batch request");
        }
    }
}