using CryptoTrading.Kucoin.DesktopInterface.Annotations;

using System;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

public interface ITickUpdater : IDisposable
{
    public event Action<Task<CallResult<object>>> OnAsyncCallError;
    public event Action<CallResult<object>> OnCallError;
    public event OnTickUpdate OnTickUpdate;

    TimeSpan UpdateInterval { get; set; }

    bool Start();

    bool Stop();

    TickUpdateSubscription Subscribe([NotNull]ITickerTarget target, [NotNull]Type targetType);

    void Unsubscribe([NotNull] TickUpdateSubscription subscription);
}