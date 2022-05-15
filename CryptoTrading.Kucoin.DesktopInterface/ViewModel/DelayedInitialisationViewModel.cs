
using System.Windows.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class DelayedInitialisationViewModel : BaseViewModel
{
    protected DelayedInitialisationViewModel()
    {
        _ = Dispatcher.CurrentDispatcher.InvokeAsync(Init, DispatcherPriority.Loaded);
    }

    protected virtual void Init()
    {
    }
}