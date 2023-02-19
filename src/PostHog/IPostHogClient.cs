namespace PostHog;

public interface IPostHogClient
{
    void Capture(CaptureRequest request);
}