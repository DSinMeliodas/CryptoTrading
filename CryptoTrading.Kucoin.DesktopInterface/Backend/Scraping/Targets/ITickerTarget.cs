using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Kucoin.Net.Clients;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public interface ITickerTarget
{
    Task<CallResult<object>> UpdateOn(KucoinClient client);
}