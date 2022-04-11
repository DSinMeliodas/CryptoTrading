using CryptoTrading.Kucoin.DesktopInterface.UseCases;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal sealed class StartCommand : UseCaseCommand
{
    public StartCommand() : base(new ApplicationStart())
    {
    }
}