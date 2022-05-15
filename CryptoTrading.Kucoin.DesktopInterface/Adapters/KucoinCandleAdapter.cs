using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Objects.Models.Spot;

namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

internal sealed class KucoinCandleAdapter : IAdapter<KucoinKline, Candle>
{
    public Candle ConvertFrom(KucoinKline value)
    {
        var factory = new Candle.CandleFactory();
        factory.SetOpenTime(value.OpenTime);
        factory.SetHighLow(value.HighPrice, value.LowPrice);
        factory.SetOpen(value.OpenPrice);
        factory.SetClose(value.ClosePrice);
        factory.SetVolume(value.Volume, value.QuoteVolume);
        return factory.Create();
    }
}