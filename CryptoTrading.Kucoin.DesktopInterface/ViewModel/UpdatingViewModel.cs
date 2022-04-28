using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

using System.Windows.Threading;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class UpdatingViewModel : BaseViewModel
{
    protected TickUpdateSubscription Subscription { get; private set; }

    private readonly ISubscriptionCallBack m_CallBack;

    protected UpdatingViewModel(ISubscriptionCallBack callBack)
    {
        m_CallBack = callBack;
        _ = Dispatcher.CurrentDispatcher.InvokeAsync(Init, DispatcherPriority.Loaded);
    }

    protected virtual void Init()
    {
    }

    protected override void Dispose(bool disposing)
    {
    }
}