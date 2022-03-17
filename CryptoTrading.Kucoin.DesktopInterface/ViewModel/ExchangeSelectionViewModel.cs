using CryptoTrading.Kucoin.DesktopInterface.Domain;

using System.Collections.ObjectModel;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : BaseViewModel
{
    private readonly ExchangeSelector m_Selector = ExchangeSelector.InPlaceInstance();

    public ObservableCollection<string> Exchanges => m_Selector.Exchanges;

    protected override void Dispose(bool disposing)
    {
    }
}
