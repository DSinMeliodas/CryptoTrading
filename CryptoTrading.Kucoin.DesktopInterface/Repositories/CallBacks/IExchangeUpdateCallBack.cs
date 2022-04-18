using System.Collections.Generic;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

public interface IExchangeUpdateCallBack
{
    void OnExchangeUpdate(IReadOnlyList<Candle> currentCandles);
}