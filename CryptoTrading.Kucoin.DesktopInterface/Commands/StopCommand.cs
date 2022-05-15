using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using System.ComponentModel;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal class StopCommand : ContextBasedUseCaseCommand<CancelEventArgs>
{
    public StopCommand() : base(new ApplicationStop())
    {
    }
}