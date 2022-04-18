using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

public interface IExchangeRepository : IDisposable
{
    Exchange GetExchange(ExchangeIdentifier exchangeId, IExchangeUpdateCallBack callBack);
}