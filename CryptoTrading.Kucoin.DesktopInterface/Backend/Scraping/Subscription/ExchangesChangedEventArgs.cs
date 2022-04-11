using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

public class ExchangesChangedEventArgs : EventArgs
{
    public IReadOnlyList<string> Exchanges { get; }
    public IReadOnlyList<string>? RemovedExchanges { get; }

    public ExchangesChangedEventArgs(IEnumerable<string> exchanges, IEnumerable<string>? removedExchanges = null)
    {
        Exchanges = exchanges.ToList();
        RemovedExchanges = removedExchanges?.ToList();
    }
}