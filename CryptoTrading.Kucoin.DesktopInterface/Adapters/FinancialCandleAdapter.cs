using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using LiveChartsCore.Defaults;

namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

internal sealed class FinancialCandleAdapter : IAdapter<Candle, FinancialPoint>
{
    public FinancialPoint ConvertFrom(Candle value) => new (value.OpenTime,(double)value.Open, (double)value.High, (double)value.Low, (double)value.Close);
}