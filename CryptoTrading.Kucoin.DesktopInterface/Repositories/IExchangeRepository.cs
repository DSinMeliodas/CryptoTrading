using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

public interface IExchangeRepository : IDisposable
{
    Task<Exchange> GetExchange(ExchangeIdentifier exchangeId, IExchangeUpdateCallBack callBack);

    Task DeleteExchange(ExchangeIdentifier exchangeId);
}