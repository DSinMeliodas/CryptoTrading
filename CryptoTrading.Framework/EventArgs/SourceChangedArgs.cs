namespace CryptoTrading.Framework.EventArgs;

internal sealed class SourceChangedArgs<TSource> : System.EventArgs
{
    public TSource OldSource { get; }

    public TSource NewSource { get; }

    public SourceChangedArgs(
        TSource oldSource,
        TSource newSource)
    {
        OldSource = oldSource;
        NewSource = newSource;
    }
}