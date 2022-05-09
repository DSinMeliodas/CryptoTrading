using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using LiveChartsCore.Defaults;

namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

internal class ExchangeCandleConverter : IAdapter<Candle, FinancialPoint>
{
    public ExchangeCandleConverter()
    {
    }

    public FinancialPoint ConvertFrom(Candle value) => new (value.OpenTime,(double)value.Open, (double)value.High, (double)value.Low, (double)value.Close);
}