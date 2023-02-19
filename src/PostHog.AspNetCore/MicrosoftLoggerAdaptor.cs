using System.Diagnostics;
using Microsoft.Extensions.Logging;
using PostHog.Internal;

namespace PostHog.AspNetCore;

internal sealed class MicrosoftLoggerAdaptor : IInternalLogger
{
    private readonly PostHogOptions _options;
    private readonly ILogger _logger;
    
    public MicrosoftLoggerAdaptor(PostHogOptions options, ILoggerFactory loggerFactory)
    {
        _options = options;
        _logger = loggerFactory.CreateLogger("PostHog");
    }

    public bool IsEnabled(InternalLogLevel logLevel)
    {
        return _options.Debug || _logger.IsEnabled(AdaptLogLevel(logLevel));
    }

    public void Log(InternalLogLevel logLevel, string message, Exception? exception = null, params object?[] args)
    {
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
#pragma warning disable CA2254
        _logger.Log(AdaptLogLevel(logLevel), exception, message, args);
#pragma warning restore CA2254
    }

    private LogLevel AdaptLogLevel(InternalLogLevel logLevel) => logLevel switch
    {
        InternalLogLevel.Debug => _options.Debug ? LogLevel.Information : LogLevel.Debug,
        InternalLogLevel.Information => LogLevel.Information,
        InternalLogLevel.Warning => LogLevel.Warning,
        InternalLogLevel.Error => LogLevel.Error,
        _ => throw new UnreachableException()
    };
}