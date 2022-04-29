using CryptoTrading.Kucoin.DesktopInterface.Adapters;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Util;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Clients;
using Kucoin.Net.Enums;
using Kucoin.Net.Interfaces.Clients.SpotApi;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal sealed partial class KucoinUpdater : IExchangeUpdater
{
    private readonly KucoinClient m_Client = new();

    private IKucoinClientSpotApiExchangeData ExchangeApi => m_Client.SpotApi.ExchangeData;

    public KucoinUpdater()
    {
    }

    public void Dispose()
    {
        m_Client?.Dispose();
    }

    public Task<IReadOnlyList<string>> GetExchangeSymbols()
    {
        var cts = new CancellationTokenSource(30000);
        var call = ExchangeApi.GetSymbolsAsync(ct: cts.Token).Result;
        ThrowHelper.ThrowIfCallError(call);
        var result = call.Data;
        var symbols = result.Select(symbol => symbol.Symbol).ToList();
        symbols.Sort();
        return Task.FromResult<IReadOnlyList<string>>(symbols);
    }

    public Task<Exchange> GetExchange(ExchangeIdentifier exchangeId, KlineInterval interval = KlineInterval.ThirtyMinutes)
    {
        var call = ExchangeApi.GetKlinesAsync(exchangeId.Symbol, interval).Result;
        ThrowHelper.ThrowIfCallError(call);
        var result = call.Data;
        var exchangeAdapter = new KucoinExchangeAdapter(exchangeId, interval);
        return Task.FromResult(exchangeAdapter.ConvertFrom(result));
    }

    public async Task<object> MakeUpdateCall(IExchangeTarget target)
    {
        ThrowHelper.ThrowIfUndefined(target.DataTargetIdentifier);
        return await CreateCallMapping()[target.DataTargetIdentifier].Invoke(target.RequestParameters);
    }

    private IReadOnlyDictionary<DataTargetIdentifier, MakeCall> CreateCallMapping()
    {
        //Methods are defined in the separate file KucoinUpdater.UpdateCalls.cs
        return new Dictionary<DataTargetIdentifier, MakeCall>
        {
            { DataTargetIdentifier.Exchange, UpdateExchangeCall },
            { DataTargetIdentifier.ExchangeSymbols, UpdateExchangeSymbols}
        };
    }
}