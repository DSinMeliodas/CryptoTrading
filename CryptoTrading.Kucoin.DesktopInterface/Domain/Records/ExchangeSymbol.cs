namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public record ExchangeSymbol(string Symbol, string TradingCurrency, string BaseCurrency)
{
    public override string ToString()
    {
        return Symbol;
    }
}