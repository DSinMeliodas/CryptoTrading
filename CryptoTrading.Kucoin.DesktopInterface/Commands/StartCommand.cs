using CryptoTrading.Kucoin.DesktopInterface.UseCases;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

public class StartCommand : UseCaseCommand
{
    public const string InPlaceInstanceId = "Inplace";

    public StartCommand(bool keepTargetAlive = false)
        : base(new ApplicationStart(), keepTargetAlive)
    {
    }
}