using CryptoTrading.Kucoin.DesktopInterface.Domain.Aggregates;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;

public sealed class Exchange : IEquatable<Exchange>
{
    public ExchangeHistory History { get; set; }

    public ExchangeSymbol Symbol { get; }

    public Exchange(ExchangeSymbol symbol, ExchangeHistory history)
    {
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        History = history ?? throw new ArgumentNullException(nameof(history));
    }

    public override string ToString() => $"{Symbol}: {History.Interval} From: {History.Course.First().OpenTime} To: {History.Course.Last().OpenTime}";

    public bool Equals(Exchange other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(Symbol, other.Symbol);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Exchange other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Symbol.GetHashCode();
    }
}