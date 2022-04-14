namespace CryptoTrading.Kucoin.DesktopInterface.Domain;

public record ExchangeIdentifier(string Identifier, string TradingCurrency, string BaseCurrency)
{
    public override string ToString()
    {
        return Identifier;
    }
}