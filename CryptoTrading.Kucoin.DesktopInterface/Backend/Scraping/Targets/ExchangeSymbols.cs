using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

internal sealed class ExchangeSymbols : IExchangeTarget
{

    public DataTargetIdentifier DataTargetIdentifier => DataTargetIdentifier.ExchangeSymbols;

    public IReadOnlyDictionary<string, object> RequestParameters { get; } = new Dictionary<string, object>();
}