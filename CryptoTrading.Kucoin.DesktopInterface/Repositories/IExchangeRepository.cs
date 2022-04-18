using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

public interface IExchangeRepository
{
    Exchange GetExchange(string exchangeId, IExchangeUpdateCallBack callBack);
}