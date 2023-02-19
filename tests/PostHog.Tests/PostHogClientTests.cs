namespace PostHog.Tests;

public sealed class PostHogClientTests
{
    private readonly Mock<IQueueManager> _queueManagerMock = new(MockBehavior.Strict);
    private readonly Mock<IInternalLogger> _internalLogger = new(MockBehavior.Strict);

    private readonly PostHogClient _sut;

    public PostHogClientTests()
    {
        _sut = new PostHogClient(_queueManagerMock.Object, _internalLogger.Object);
    }
}