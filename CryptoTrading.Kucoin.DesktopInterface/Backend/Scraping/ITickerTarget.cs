using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Kucoin.Net.Clients;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

public interface ITickerTarget
{
    Task<CallResult<object>> UpdateOn(KucoinClient client);
}