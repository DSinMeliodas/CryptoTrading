using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

public interface IExchangeRequest
{
    string ExchangeSymbol { get; }

    IExchangeUpdateCallBack UpdateCallBack { get; }
}