using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain;

internal class ExchangeSelector
{
    private const string InPlaceInstanceId = nameof(InPlaceInstance);

    private static readonly Dictionary<string, ExchangeSelector> Selectors = new();

    private readonly ITickUpdater m_TickUpdater;

    public ObservableCollection<string> Exchanges { get; private set; }

    private ExchangeSelector(ITickUpdater updater)
    {
        m_TickUpdater = updater;
    }

    public static ExchangeSelector InPlaceInstance()
    {
        if(Selectors.TryGetValue(InPlaceInstanceId, out var selector)) {
            return selector;
        }
        selector = new ExchangeSelector(null);
        Selectors.Add(InPlaceInstanceId, selector);
        return selector;
    }
}
