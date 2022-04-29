using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Enums;
using Kucoin.Net.Objects.Models.Spot;

using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

internal sealed class KucoinExchangeAdapter : IAdapter<IEnumerable<KucoinKline>, Exchange>
{
    private readonly ExchangeIdentifier m_Identifier;
    private readonly KlineInterval m_Interval;

    public KucoinExchangeAdapter(ExchangeIdentifier identifier, KlineInterval interval)
    {
        m_Identifier = identifier;
        m_Interval = interval;
    }

    public Exchange ConvertFrom(IEnumerable<KucoinKline> klines)
    {
        var candleAdapter = new KucoinCandleAdapter();
        var candles = klines.Select(candleAdapter.ConvertFrom).ToArray();
        return new Exchange(m_Identifier, m_Interval, candles);
    }
}