using CryptoTrading.Kucoin.DesktopInterface.Domain;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : BaseViewModel
{
    private readonly ExchangeSelector Selector = ExchangeSelector.InPlaceInstance();

    public ObservableCollection<string> Exchanges => Selector.Exchanges;

    protected override void Dispose(bool disposing)
    {
    }
}
