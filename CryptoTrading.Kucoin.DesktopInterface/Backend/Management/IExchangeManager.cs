using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Management;


public delegate void ExchangeChanged(object sender, ExchangeChangedArgs args);

public interface IExchangeManager : IDisposable
{
    event ExchangeChanged OnOpenedExchangesChanged;

    ExchangeSymbol CurrentExchangeSymbol { get; }

    int CurrentIndex { get; }

    void CloseCurrentExchange();

    public void CloseExchange(ExchangeSymbol exchangeId);

    bool IsOpen(ExchangeSymbol target);

    void OpenExchange(Exchange exchange);

    void SetOpenedAsCurrent(ExchangeSymbol exchangeId);

    void UpdateExchange(Exchange obj);
}