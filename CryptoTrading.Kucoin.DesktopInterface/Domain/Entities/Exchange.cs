using CryptoTrading.Kucoin.DesktopInterface.Domain.Aggregates;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;

public class Exchange
{
    public ExchangeHistory History { get; set; }

    public ExchangeSymbol Symbol { get; }

    public Exchange(ExchangeSymbol symbol, ExchangeHistory history)
    {
        Symbol = symbol;
        History = history;
    }

    public override string ToString() => $"{Symbol}: {History.Interval} From: {History.Course.First().OpenTime} To: {History.Course.Last().OpenTime}";
}