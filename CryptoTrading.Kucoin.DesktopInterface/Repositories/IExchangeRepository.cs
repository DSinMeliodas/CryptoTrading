using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

public interface IExchangeRepository : IDisposable
{
    Task<IReadOnlyList<string>> GetAvailableExchanges(IExchangeSymbolsUpdateCallBack callBack);

    Task<Exchange> GetExchange(ExchangeSymbol symbol, IExchangeUpdateCallBack callBack);

    Task DeleteExchange(ExchangeSymbol symbol);
}