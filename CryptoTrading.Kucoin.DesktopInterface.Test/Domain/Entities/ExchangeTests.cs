using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Enums;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Aggregates;
using NUnit.Framework.Constraints;

namespace CryptoTrading.Kucoin.DesktopInterface.Test.Domain.Entities;

[TestFixture]
public sealed class ExchangeTests
{
    [Test]
    public void ThrowsNullExceptionsOnConstructorCall()
    {
        var symbol = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT").Create();
        var history = new ExchangeHistory(KlineInterval.ThirtyMinutes, Array.Empty<Candle>());
        _ = Assert.Throws<ArgumentNullException>(() => _ = new Exchange(null, history));
        _ = Assert.Throws<ArgumentNullException>(() => _ = new Exchange(symbol, null));

    }

    [Test]
    public void AreEqualTest()
    {
        var exchangeSymbol = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT").Create();
        var exchange1 = new Exchange(exchangeSymbol, new(KlineInterval.ThirtyMinutes, Array.Empty<Candle>()));
        var exchange2 = new Exchange(exchangeSymbol, new(KlineInterval.EightHours, new List<Candle>()));
        Assert.AreEqual(exchange1, exchange2);
    }

    [Test]
    public void AreNotEqualTest()
    {
        var exchangeSymbol1 = new ExchangeSymbol.ExchangeSymbolFactory("BTC-USDT").Create();
        var exchangeSymbol2 = new ExchangeSymbol.ExchangeSymbolFactory("BTC-UST").Create();
        var exchange1 = new Exchange(exchangeSymbol1, new(KlineInterval.ThirtyMinutes, Array.Empty<Candle>()));
        var exchange2 = new Exchange(exchangeSymbol2, new(KlineInterval.EightHours, new List<Candle>()));
        Assert.AreNotEqual(exchange1, exchange2);
    }
}