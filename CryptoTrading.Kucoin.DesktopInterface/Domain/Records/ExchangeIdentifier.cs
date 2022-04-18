namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public record ExchangeIdentifier(string Symbol, string TradingCurrency, string BaseCurrency)
{
    public override string ToString()
    {
        return Symbol;
    }
}