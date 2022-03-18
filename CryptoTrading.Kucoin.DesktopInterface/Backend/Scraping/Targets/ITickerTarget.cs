using System;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Kucoin.Net.Clients;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public interface ITickerTarget
{
    Type ResultType { get; }

    Task<CallResult<object>> UpdateOn(KucoinClient client);
}