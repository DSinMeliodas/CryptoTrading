using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

internal sealed class ExchangeRequest : IExchangeRequest
{

    public string ExchangeId { get; }

    public IExchangeUpdateCallBack UpdateCallBack { get; }

    public ExchangeRequest(string exchangeId, IExchangeUpdateCallBack updateCallBack)
    {
        ExchangeId = exchangeId;
        UpdateCallBack = updateCallBack;
    }
}