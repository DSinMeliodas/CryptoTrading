
using System.Windows.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class DelaysedInitialisationViewModel : BaseViewModel
{
    protected DelaysedInitialisationViewModel()
    {
        _ = Dispatcher.CurrentDispatcher.InvokeAsync(Init, DispatcherPriority.Loaded);
    }

    protected virtual void Init()
    {
    }
}