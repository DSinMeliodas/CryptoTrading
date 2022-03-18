using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Kucoin.Net.Clients;
using Kucoin.Net.Objects.Models.Spot;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public sealed class ExchangeSymbols : ITickerTarget
{
    public Type ResultType { get; } = typeof(IEnumerable<KucoinSymbol>);

    public async Task<CallResult<object>> UpdateOn(KucoinClient client)
    {
        var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();
        return result.As((object)result.Data);
    }
}