using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Domain;

using Kucoin.Net.Clients;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public sealed class ExchangeTickUpdaterTarget : ITickUpdaterTarget
{
    private const string UpdaterIdentifierFormat = @"{0}-Updater";

    private readonly ExchangeIdentifier m_Identifier;

    public Type ResultType => typeof(IEnumerable<Kline>);

    public string UpdaterId { get; }

    public ExchangeTickUpdaterTarget(ExchangeIdentifier identifier)
    {
        m_Identifier = identifier;
        UpdaterId = string.Format(UpdaterIdentifierFormat, m_Identifier.Symbol);
    }

    public async Task<CallResult<object>> UpdateOn(KucoinClient client)
    {
        var result = await client.SpotApi.CommonSpotClient.GetKlinesAsync(m_Identifier.Symbol, TimeSpan.MaxValue);
        return result.As((object)result.Data);
    }
}