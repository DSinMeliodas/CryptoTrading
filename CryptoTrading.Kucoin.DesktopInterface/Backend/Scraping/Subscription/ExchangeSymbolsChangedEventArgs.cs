using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

public class ExchangeSymbolsChangedEventArgs : EventArgs
{
    public IReadOnlyList<string> Exchanges { get; }
    public IReadOnlyList<string>? RemovedExchanges { get; }

    public ExchangeSymbolsChangedEventArgs(IEnumerable<string> currentSymbols, IEnumerable<string>? removedSymbols = null)
    {
        Exchanges = currentSymbols.ToList();
        RemovedExchanges = removedSymbols?.ToList();
    }
}