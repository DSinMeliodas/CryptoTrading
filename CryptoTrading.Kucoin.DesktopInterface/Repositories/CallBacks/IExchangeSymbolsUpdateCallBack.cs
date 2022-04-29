using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

public interface IExchangeSymbolsUpdateCallBack
{
    void NotifySymbolsChanged(IReadOnlyList<string> currentSymbols, IReadOnlySet<string> removedSymbols = null);
}