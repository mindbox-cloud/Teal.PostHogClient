using PostHog;

var postHogOptions = new PostHogOptions
{
    ApiKey = "<your_api_key>",
    Debug = true,
    FlushInterval = TimeSpan.FromSeconds(1)
};

await using var postHogClient = new PostHogClient(postHogOptions);

while (Console.ReadKey(true).Key != ConsoleKey.Q)
{
    var captureRequest = new CaptureRequest("ConsoleUser", "TestEvent");
    postHogClient.Capture(captureRequest);
}