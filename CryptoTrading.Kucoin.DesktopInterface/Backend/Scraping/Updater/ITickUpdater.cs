using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

using System;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface ITickUpdater : IDisposable
{
    public event Action<Task<CallResult<object>>> OnAsyncCallError;
    public event Action<CallResult<object>> OnCallError;
    public event OnTickUpdate OnTickUpdate;

    TimeSpan UpdateInterval { get; set; }

    bool Start();

    bool Stop();

    TickUpdateSubscription Subscribe(ITickerTarget target, ISubscriptionCallBack subscriptionCallBack);

    void Unsubscribe(TickUpdateSubscription subscription);
}