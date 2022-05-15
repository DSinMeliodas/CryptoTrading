using System;
using System.Collections.Generic;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

public sealed class LoadAvailableExchangesCallBack : IExchangeSymbolsUpdateCallBack
{
    public event EventHandler<ExchangeSymbolsChangedEventArgs> OnSymbolsChanged;

    public void NotifySymbolsChanged(IReadOnlyList<string> currentSymbols, IReadOnlySet<string> removedSymbols = null)
    {
        OnSymbolsChanged?.Invoke(this, new ExchangeSymbolsChangedEventArgs(currentSymbols, removedSymbols));
    }
}