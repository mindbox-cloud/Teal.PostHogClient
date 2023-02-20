namespace TealInc.PostHogClient.Internal;

public abstract class PeriodicTask
{
    private Task _timerTask = default!;
    private readonly PeriodicTimer _timer;
    private readonly CancellationTokenSource _cts = new();

    protected PeriodicTask(TimeSpan interval)
    {
        _timer = new PeriodicTimer(interval);
        Start();
    }

    private void Start()
    {
        _timerTask = DoWorkAsync().ContinueWith(HandleError);
    }

    private void HandleError(Task failedTask)
    {
        if (!failedTask.IsFaulted)
        {
            return;
        }
    
        Start();
    }

    public async Task StopAsync()
    {
        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();

        await OnTickAsync();
    }

    protected abstract Task OnTickAsync();

    private async Task DoWorkAsync()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                await OnTickAsync();
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}