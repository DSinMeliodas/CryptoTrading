using CryptoTrading.Kucoin.DesktopInterface.Domain.Aggregates;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Enums;
using Kucoin.Net.Objects.Models.Spot;

using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

internal sealed class KucoinExchangeAdapter : IAdapter<IEnumerable<KucoinKline>, Exchange>
{
    private readonly ExchangeSymbol m_Identifier;
    private readonly KlineInterval m_Interval;

    public KucoinExchangeAdapter(ExchangeSymbol identifier, KlineInterval interval)
    {
        m_Identifier = identifier;
        m_Interval = interval;
    }

    public Exchange ConvertFrom(IEnumerable<KucoinKline> klines)
    {
        var candleAdapter = new KucoinCandleAdapter();
        var candles = klines.Select(candleAdapter.ConvertFrom).ToArray();
        return new Exchange(m_Identifier, new ExchangeHistory(m_Interval, candles));
    }
}