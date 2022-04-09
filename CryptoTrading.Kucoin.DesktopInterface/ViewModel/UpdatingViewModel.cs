using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class UpdatingViewModel : BaseViewModel
{

    protected TickUpdateSubscription Subscription { get; }

    protected UpdatingViewModel(ITickUpdaterTarget target, ISubscriptionCallBack callBack)
    {
        Subscription = DataHub.Instance.Subscribe(target, callBack);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
        DataHub.Instance.Unsubscribe(Subscription);
    }
}