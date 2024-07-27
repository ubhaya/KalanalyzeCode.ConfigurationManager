using Microsoft.AspNetCore.Components;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.Layout;

public abstract class ApplicationComponentBase : ComponentBase, IAsyncDisposable
{
    private CancellationTokenSource? _cancellationTokenSource;

    protected CancellationToken CancellationToken =>
        (_cancellationTokenSource ??= new CancellationTokenSource()).Token;
    
    public ValueTask DisposeAsync()
    {
        if (_cancellationTokenSource is null) return ValueTask.CompletedTask;
        _cancellationTokenSource.CancelAsync();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
        return ValueTask.CompletedTask;
    }
}