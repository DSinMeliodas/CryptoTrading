using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

public interface ISubscriptionCallBack
{
    TickUpdateSubscription Subscription { set; }

    void OnAsyncCallError(Task<CallResult<object>> obj);

    void OnCallError(CallResult<object> obj);

    void OnTickUpdate(ITickUpdater sender, TickUpdateEventArgs args);
}