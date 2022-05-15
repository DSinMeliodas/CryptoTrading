using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public interface IExchangeTarget
{
    DataTargetIdentifier DataTargetIdentifier { get; }

    IReadOnlyDictionary<string, object> RequestParameters { get; }
}