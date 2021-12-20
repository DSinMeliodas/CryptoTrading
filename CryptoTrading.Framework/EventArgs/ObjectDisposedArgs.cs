namespace CryptoTrading.Framework.EventArgs;

internal sealed class ObjectDisposedArgs<TDisposed> : System.EventArgs
    where TDisposed : IDisposable
{
    public TDisposed DisposedObject { get; }

    public ObjectDisposedArgs(TDisposed disposedObject)
    {
        DisposedObject = disposedObject;
    }
}