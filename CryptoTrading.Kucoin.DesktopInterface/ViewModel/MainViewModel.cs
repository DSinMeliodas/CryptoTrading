using CryptoTrading.Kucoin.DesktopInterface.Commands;

using System.Windows.Input;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal class MainViewModel : BaseViewModel
{
    public ICommand OnShutDown => new StopCommand();

    public ICommand OnStart => new StartCommand();

    protected override void Dispose(bool disposing)
    {
    }
}