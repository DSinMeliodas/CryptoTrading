using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using NUnit.Framework;

namespace CryptoTrading.Kucoin.DesktopInterface.Test.Domain.Records;

public sealed class ExchangeSymbolTests
{
    [Test]
    public void AreEqualTest()
    {
        var rawSymbol = "BTC-USDT";
        var symbol1 = new ExchangeSymbol.ExchangeSymbolFactory(rawSymbol).Create();
        var symbol2 = new ExchangeSymbol.ExchangeSymbolFactory(rawSymbol).Create();
        Assert.AreEqual(symbol1, symbol2);
    }

    [Test]
    public void AreNotEqualTest()
    {
        var rawSymbol1 = "BTC-USDT";
        var rawSymbol2 = "BTC-UST";
        var symbol1 = new ExchangeSymbol.ExchangeSymbolFactory(rawSymbol1).Create();
        var symbol2 = new ExchangeSymbol.ExchangeSymbolFactory(rawSymbol2).Create();
        Assert.AreNotEqual(symbol1, symbol2);
    }
}