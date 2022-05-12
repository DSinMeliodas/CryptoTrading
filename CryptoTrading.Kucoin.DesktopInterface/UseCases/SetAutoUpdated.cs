using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal sealed class SetAutoUpdated : IContextBasedUseCase<IUpdateIntervalSettings>
{
    public void Execute(IUpdateIntervalSettings context) => context.ChangeAutoUpdated(!context.IsAutoUpdated);
}