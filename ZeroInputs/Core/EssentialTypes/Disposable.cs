public abstract class Disposable : IDisposable
{
    public bool IsDisposed { get; private set; }

    ~Disposable() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void ReleaseUnmanagedResources()
    {
    }

    protected virtual void ReleaseManagedResources()
    {
    }

    private void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        IsDisposed = true;

        ReleaseUnmanagedResources();

        if (disposing)
            ReleaseManagedResources();
    }
}