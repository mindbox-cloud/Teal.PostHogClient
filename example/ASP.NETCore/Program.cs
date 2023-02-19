using PostHog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddPostHog(options =>
{
    options.ApiKey = "<your_api_key>";
    options.Debug = true;
    options.FlushInterval = TimeSpan.FromSeconds(1);
});

var app = builder.Build();

app.MapGet("/", () => "Hello PostHog!");

app.MapGet("/event", (HttpContext context, IPostHogClient postHog) =>
{
    var ipAddress = (context.Connection.RemoteIpAddress ?? context.Connection.LocalIpAddress)?.ToString() ?? "Unknown";
    postHog.Capture(new CaptureRequest(ipAddress, "TestEvent"));
});

app.Run();