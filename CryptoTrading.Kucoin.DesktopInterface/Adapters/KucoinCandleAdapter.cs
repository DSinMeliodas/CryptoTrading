using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Objects.Models.Spot;

namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

internal sealed class KucoinCandleAdapter : IAdapter<KucoinKline, Candle>
{
    public Candle ConvertFrom(KucoinKline value)
    {
        return new (
            value.OpenTime,
            value.OpenPrice,
            value.ClosePrice,
            value.HighPrice,
            value.LowPrice,
            value.Volume,
            value.QuoteVolume);
    }
}