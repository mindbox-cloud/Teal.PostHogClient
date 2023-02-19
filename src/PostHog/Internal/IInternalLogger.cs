namespace PostHog.Internal;

internal interface IInternalLogger
{
    bool IsEnabled(InternalLogLevel logLevel);
    void Log(InternalLogLevel logLevel, string message, Exception? exception = null, params object?[] args);
}