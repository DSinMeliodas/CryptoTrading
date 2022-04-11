using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

using System.Windows.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class UpdatingViewModel : BaseViewModel
{
    protected TickUpdateSubscription Subscription { get; private set; }

    private readonly ISubscriptionCallBack m_CallBack;
    private readonly ITickUpdaterTarget m_Target;

    protected UpdatingViewModel(ITickUpdaterTarget target, ISubscriptionCallBack callBack)
    {
        m_CallBack = callBack;
        m_Target = target;
        _ = Dispatcher.CurrentDispatcher.InvokeAsync(Init, DispatcherPriority.Loaded | DispatcherPriority.ApplicationIdle);
    }

    protected virtual void Init()
    {
        Subscription = DataHub.Instance.Subscribe(m_Target, m_CallBack);
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