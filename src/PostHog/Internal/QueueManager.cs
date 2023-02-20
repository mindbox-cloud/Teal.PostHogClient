using System.Collections.Concurrent;

namespace PostHog.Internal;

internal interface IQueueManager
{
    void Enqueue(PostHogEvent postHogEvent);
    Task StopAsync();
}

internal sealed class QueueManager : PeriodicTask, IQueueManager
{
    private readonly ConcurrentQueue<PostHogEvent> _queue = new();

    private readonly PostHogOptions _options;
    private readonly IPostHogApi _api;
    private readonly IInternalLogger _internalLogger;

    public QueueManager(PostHogOptions options, IPostHogApi api, IInternalLogger internalLogger) : base(options.FlushInterval)
    {
        _options = options;
        _api = api;
        _internalLogger = internalLogger;
    }

    public void Enqueue(PostHogEvent postHogEvent)
    {
        _queue.Enqueue(postHogEvent);

        if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
        {
            _internalLogger.Log(InternalLogLevel.Debug, "Enqueued {0} with ts {1}", null, postHogEvent.Event, postHogEvent.Timestamp);
        }
    }

    protected override async Task OnTickAsync()
    {
        if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
        {
            _internalLogger.Log(InternalLogLevel.Debug, "Flushing queue");
        }

        var events = new List<PostHogEvent>();
        while (_queue.TryDequeue(out var postHogEvent))
        {
            events.Add(postHogEvent);
            
            if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
            {
                _internalLogger.Log(InternalLogLevel.Debug, "Dequeued {0} with ts {1}", null, postHogEvent.Event, postHogEvent.Timestamp);
            }
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
            if (_internalLogger.IsEnabled(InternalLogLevel.Error))
            {
                _internalLogger.Log(InternalLogLevel.Error, "Error occured while sending batch request:", e);
            }
        }
    }
}