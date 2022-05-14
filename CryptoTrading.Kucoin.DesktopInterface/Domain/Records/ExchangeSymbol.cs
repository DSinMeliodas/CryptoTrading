namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public partial record ExchangeSymbol
{
    public string Symbol { get; }
    public string TradingCurrency { get; }
    public string BaseCurrency { get; }

    private ExchangeSymbol(string symbol, string tradingCurrency, string baseCurrency)
    {
        Symbol = symbol;
        TradingCurrency = tradingCurrency;
        BaseCurrency = baseCurrency;
    }

    public override string ToString()
    {
        return Symbol;
    }

    public void Deconstruct(out string Symbol, out string TradingCurrency, out string BaseCurrency)
    {
        Symbol = this.Symbol;
        TradingCurrency = this.TradingCurrency;
        BaseCurrency = this.BaseCurrency;
    }
}