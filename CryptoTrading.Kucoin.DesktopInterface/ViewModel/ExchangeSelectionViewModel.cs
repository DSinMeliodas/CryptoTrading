
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using Kucoin.Net.Objects.Models.Spot;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    public ObservableCollection<string> Exchanges { get; } = new();

    public ExchangeSelectionViewModel()
    {
        _ = Updater.Subscribe(new ExchangeSymbols(), typeof(IEnumerable<KucoinSymbol>));
    }
}
