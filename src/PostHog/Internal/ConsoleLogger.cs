namespace PostHog.Internal;

internal sealed class ConsoleLogger : IInternalLogger
{
    private readonly PostHogOptions _options;

    public ConsoleLogger(PostHogOptions options)
    {
        _options = options;
    }
    
    public bool IsEnabled(InternalLogLevel logLevel)
    {
        return _options.Debug || logLevel > InternalLogLevel.Debug;
    }

    public void Log(InternalLogLevel logLevel, string message, Exception? exception = null, params object?[] args)
    {
        var text = args.Length == 0 ? message : string.Format(message, args);
    
        var completeMessage = exception == null
            ? $"PostHog ({logLevel}): {text}"
            : $"PostHog ({logLevel}): {text}{Environment.NewLine}{exception}";

        Console.WriteLine(completeMessage);
    }
}