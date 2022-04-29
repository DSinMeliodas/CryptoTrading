
using System.Windows.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class UpdatingViewModel : BaseViewModel
{
    protected UpdatingViewModel()
    {
        _ = Dispatcher.CurrentDispatcher.InvokeAsync(Init, DispatcherPriority.Loaded);
    }

    protected virtual void Init()
    {
    }

    protected override void Dispose(bool disposing)
    {
    }
}