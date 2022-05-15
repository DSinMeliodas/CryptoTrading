using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal sealed class SetAutoUpdateCommand : ContextBasedUseCaseCommand<IUpdateIntervalSettings>
{
    public SetAutoUpdateCommand() : base(new SetAutoUpdated())
    {
    }
}