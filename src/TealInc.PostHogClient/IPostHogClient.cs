namespace TealInc.PostHogClient;

public interface IPostHogClient
{
    void Capture(CaptureRequest request);
}