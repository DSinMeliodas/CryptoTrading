using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

internal sealed record ExchangeRequest(string ExchangeSymbol, IExchangeUpdateCallBack UpdateCallBack) : IExchangeRequest;