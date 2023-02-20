using PostHog;
using PostHog.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class PostHogConfiguration
{
    public static IServiceCollection AddPostHog(this IServiceCollection services, Action<PostHogOptions> configure)
    {
        var options = new PostHogOptions();
        configure(options);
        
        services.AddHttpClient();
        
        services.AddSingleton(options);
        services.AddSingleton<IPostHogApi, PostHogApi>();
        services.AddSingleton<IQueueManager, QueueManager>();
        services.AddSingleton<IPostHogClient, PostHogClient>();

        return services;
    }
}