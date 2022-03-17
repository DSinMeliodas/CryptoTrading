using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class UpdatingViewModel : BaseViewModel
{
    protected ITickUpdater Updater { get; set; } = DataHub.UseInPlaceUpdater(false);

    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
        Updater?.Dispose();
    }
}