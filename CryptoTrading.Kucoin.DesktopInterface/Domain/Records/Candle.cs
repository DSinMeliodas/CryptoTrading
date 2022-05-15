using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public partial record Candle
{
    public DateTime OpenTime { get; init; }
    public decimal Open { get; init; }
    public decimal Close { get; init; }
    public decimal High { get; init; }
    public decimal Low { get; init; }
    public decimal BaseVolume { get; init; }
    public decimal ForeignVolume { get; init; }

    private Candle(DateTime openTime, decimal open, decimal close, decimal high, decimal low, decimal baseVolume,
        decimal foreignVolume)
    {
        OpenTime = openTime;
        Open = open;
        Close = close;
        High = high;
        Low = low;
        BaseVolume = baseVolume;
        ForeignVolume = foreignVolume;
    }

    public void Deconstruct(out DateTime OpenTime, out decimal Open, out decimal Close, out decimal High, out decimal Low, out decimal BaseVolume, out decimal ForeignVolume)
    {
        OpenTime = this.OpenTime;
        Open = this.Open;
        Close = this.Close;
        High = this.High;
        Low = this.Low;
        BaseVolume = this.BaseVolume;
        ForeignVolume = this.ForeignVolume;
    }
}