using System.Threading;

namespace CryptoTrading.Framework.Util.Cancellation;

internal static class CancellationTokenUtil
{
    public static CancellationTokenSource CreateCancelledSource()
    {
        var cancelledToken = new CancellationToken(true);
        return CancellationTokenSource.CreateLinkedTokenSource(cancelledToken);
    }
}