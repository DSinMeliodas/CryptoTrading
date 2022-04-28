using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Management;

public sealed class ExchangeChangedArgs
{
    public int CurrentIndex { get; }

    public IReadOnlyList<Exchange> OpenedExchanges { get; }

    public ChangeAction Action { get; }

    public ExchangeChangedArgs(int currentIndex, IReadOnlyList<Exchange> openedExchanges, ChangeAction action)
    {
        CurrentIndex = currentIndex;
        OpenedExchanges = openedExchanges;
        Action = action;
    }
}