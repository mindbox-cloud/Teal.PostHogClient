using PostHog.Internal;

namespace PostHog;

public sealed class PostHogClient : IPostHogClient, IAsyncDisposable
{
    private readonly IQueueManager _queueManager;
    private readonly IInternalLogger _internalLogger;

    internal PostHogClient(IQueueManager queueManager, IInternalLogger internalLogger)
    {
        _queueManager = queueManager;
        _internalLogger = internalLogger;
        
        if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
        {
            _internalLogger.Log(InternalLogLevel.Debug, "Started listening for events");
        }
    }

    public PostHogClient(PostHogOptions options)
    {
        _internalLogger = new ConsoleLogger(options);
        
        var postHogApi = new PostHogApi(options, _internalLogger);
        _queueManager = new QueueManager(options, postHogApi, _internalLogger);

        if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
        {
            _internalLogger.Log(InternalLogLevel.Debug, "Started listening for events");
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _queueManager.StopAsync();

        if (_internalLogger.IsEnabled(InternalLogLevel.Debug))
        {
            _internalLogger.Log(InternalLogLevel.Debug, "Stopped listening for events");
        }
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