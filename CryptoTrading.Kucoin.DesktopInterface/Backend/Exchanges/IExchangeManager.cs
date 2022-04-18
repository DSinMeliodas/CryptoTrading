using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Exchange;

public interface IExchangeManager
{
    public ExchangeIdentifier CurrentExchangeIdentifier { get; }
    public IReadOnlySet<ExchangeIdentifier> OpenExchangeIdentifiers { get; }

    void SetOpenedAsCurrent(ExchangeIdentifier exchangeIdentifier);

    void OpenExchange(ExchangeIdentifier exchangeIdentifier);
}