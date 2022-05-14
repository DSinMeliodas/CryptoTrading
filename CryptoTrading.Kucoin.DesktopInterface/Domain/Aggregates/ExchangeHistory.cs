using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Enums;

using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Aggregates;

public class ExchangeHistory
{
    public KlineInterval Interval { get; }

    public IReadOnlyList<Candle> Course { get; }
    
    public ExchangeHistory(KlineInterval interval, IReadOnlyList<Candle> course)
    {
        Interval = interval;
        Course = course;
    }
}