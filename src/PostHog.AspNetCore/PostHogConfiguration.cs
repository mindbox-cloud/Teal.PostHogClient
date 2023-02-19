using PostHog;
using PostHog.AspNetCore;
using PostHog.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class PostHogConfiguration
{
    public static IServiceCollection AddPostHog(this IServiceCollection services, Action<PostHogOptions> configure)
    {
        var options = new PostHogOptions();
        configure(options);
        
        services.AddSingleton(options);
        services.AddSingleton<IInternalLogger, MicrosoftLoggerAdaptor>();
        services.AddSingleton<IPostHogApi, PostHogApi>();
        services.AddSingleton<IQueueManager, QueueManager>();
        services.AddSingleton<IPostHogClient, PostHogClient>(
            sp => new PostHogClient(
                sp.GetRequiredService<IQueueManager>(),
                sp.GetRequiredService<IInternalLogger>())
        );

        return services;
    }
}