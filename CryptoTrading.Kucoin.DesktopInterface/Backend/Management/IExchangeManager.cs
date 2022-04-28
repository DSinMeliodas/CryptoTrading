using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Management;


public delegate void ExchangeChanged(object sender, ExchangeChangedArgs args);

public interface IExchangeManager : IDisposable
{
    event ExchangeChanged OnOpenedExchangesChanged;

    ExchangeIdentifier CurrentExchangeIdentifier { get; }

    int CurrentIndex { get; }

    void CloseCurrentExchange();

    public void CloseExchange(ExchangeIdentifier exchangeId);

    bool IsOpen(ExchangeIdentifier target);

    void OpenExchange(Exchange exchange);

    void SetOpenedAsCurrent(ExchangeIdentifier exchangeId);

    void UpdateExchange(Exchange obj);
}