using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Kucoin.Net.Clients;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public sealed class ExchangeSymbols : ITickerTarget
{
    public async Task<CallResult<object>> UpdateOn(KucoinClient client)
    {
        var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();
        return result.As((object)result.Data);
    }
}