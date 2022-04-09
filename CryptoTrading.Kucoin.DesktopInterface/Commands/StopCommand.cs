using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using System.ComponentModel;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal class StopCommand : ContextBasedUseCaseCommand<CancelEventArgs>
{
    public StopCommand(bool keepTargetAlive = false)
        : base(new ApplicationStop(), keepTargetAlive)
    {
    }
}