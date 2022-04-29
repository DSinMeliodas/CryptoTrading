using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using Kucoin.Net.Enums;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface IExchangeUpdater : IDisposable
{
    Task<IReadOnlyList<string>> GetExchangeSymbols();

    Task<Exchange> GetExchange(ExchangeIdentifier exchangeId, KlineInterval interval = KlineInterval.ThirtyMinutes);

    Task<object> MakeUpdateCall(IExchangeTarget target);
}