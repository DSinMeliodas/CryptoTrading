
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

public interface ISubscriptionCallBack
{
    void OnTickUpdate(TickUpdateEventArgs args);
}