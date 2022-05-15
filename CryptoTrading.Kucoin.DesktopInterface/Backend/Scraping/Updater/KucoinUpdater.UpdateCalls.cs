using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal partial class KucoinUpdater
{

    private delegate Task<object> MakeCall(IReadOnlyDictionary<string, object> parameters);

    private async Task<object> UpdateExchangeCall(IReadOnlyDictionary<string, object> parameters)
    {
        var symbol = (ExchangeSymbol)parameters[TargetParameterNames.ExchangeSymbolParameter];
        var klineInterval = (KlineInterval)parameters[TargetParameterNames.ExchangeSymbolIntervalParameter];
        return await GetExchange(symbol, klineInterval);
    }

    private async Task<object> UpdateExchangeSymbols(IReadOnlyDictionary<string, object> _) => await GetExchangeSymbols();
}