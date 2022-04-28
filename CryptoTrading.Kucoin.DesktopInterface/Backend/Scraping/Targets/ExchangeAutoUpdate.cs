﻿using System.Collections.Generic;

using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

internal class ExchangeAutoUpdate : IExchangeTarget
{
    public DataTargetIdentifier DataTargetIdentifier => DataTargetIdentifier.Exchange;

    public IReadOnlyDictionary<string, object> RequestParameters { get; }

    public ExchangeAutoUpdate(ExchangeIdentifier exchangeId)
    {
        RequestParameters = new Dictionary<string, object>
        {
            { TargetParameterNames.ExchangeSymbolParameter, exchangeId.Symbol }
        };
    }
}