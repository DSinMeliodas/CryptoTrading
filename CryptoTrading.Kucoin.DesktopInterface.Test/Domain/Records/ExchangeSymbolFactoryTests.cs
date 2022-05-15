using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using NUnit.Framework;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Test.Domain.Records;

[TestFixture]
public sealed class ExchangeSymbolFactoryTests
{
    [Test]
    public void ThrowsNullExceptionsOnConstructorCall()
    {
        Assert.Throws<ArgumentNullException>(() => new ExchangeSymbol.ExchangeSymbolFactory(null));
    }

    [Test]
    public void ThrowsArgumentExceptionOnRegexMismatch()
    {
        Assert.Throws<ArgumentException>(() => new ExchangeSymbol.ExchangeSymbolFactory(""));
    }

    [Test]
    public void ContructorSucceedsOnRegexMatch()
    {
        Assert.DoesNotThrow(() => new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT"));
    }

    [Test]
    public void CreatesTheEqualObjectsForMultipleCalls()
    {
        var factory = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT");
        Assert.AreEqual(factory.Create(), factory.Create());
    }

    [Test]
    public void CreatesDifferentInstancesForMultipleCalls()
    {
        var factory1 = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT");
        var factory2 = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT");
        Assert.AreNotSame(factory1.Create(), factory2.Create());
    }

    [Test]
    public void CreatesEqualObjectsForCallsFromDiffrentInstances()
    {
        var factory1 = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT");
        var factory2 = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT");
        Assert.AreEqual(factory1.Create(), factory2.Create());
    }
}