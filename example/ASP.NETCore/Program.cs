using PostHog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddPostHog(options =>
{
    options.ApiKey = "phc_jodH1ObqDzOtkci4Ve2fDImjmhBYWb6LnjCMG9zLk8u";
    options.FlushInterval = TimeSpan.FromSeconds(1);
});

var app = builder.Build();

app.MapGet("/", () => "Hello PostHog!");

app.MapGet("/event", (IPostHogClient postHog) =>
{
    var captureRequest = new CaptureRequest("WebUser", "TestEvent");
    postHog.Capture(captureRequest);
});

app.Run();