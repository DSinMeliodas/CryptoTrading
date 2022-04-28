using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

internal sealed class KucoinExchangeRepository : IExchangeRepository
{
    public static IExchangeRepository SingletonInstance { get; } = new KucoinExchangeRepository();

    private readonly Dictionary<ExchangeIdentifier, Exchange> m_Exchanges = new ();
    private readonly Dictionary<ExchangeIdentifier, List<IExchangeUpdateCallBack>> m_RegisteredCallBacks = new();
    private readonly Dictionary<ExchangeIdentifier, TickUpdateSubscription> m_UpdateSubscriptions = new ();
    private readonly ITickUpdater m_KucoinTickUpdater = new KucoinTickUpdater();

    private KucoinExchangeRepository()
    {
    }

    public void Dispose()
    {
    }

    public async Task<Exchange> GetExchange(ExchangeIdentifier exchangeId, IExchangeUpdateCallBack callBack)
    {
        if (m_Exchanges.TryGetValue(exchangeId, out var exchange))
        {
            m_RegisteredCallBacks[exchangeId].Add(callBack);
            return exchange;
        }
        return await RegisterAndLoadInitial(exchangeId, callBack);
    }

    private async Task<Exchange> RegisterAndLoadInitial(ExchangeIdentifier exchangeId, IExchangeUpdateCallBack callBack)
    {
        var callBacks = new List<IExchangeUpdateCallBack> { callBack };
        var subscription = m_KucoinTickUpdater.Subscribe(new ExchangeAutoUpdate(exchangeId), new ExchangeUpdate(callBacks));
        m_RegisteredCallBacks[exchangeId] = callBacks;
        m_UpdateSubscriptions[exchangeId] = subscription;
        return await m_KucoinTickUpdater.BaseUpdater.GetExchange(exchangeId);
    }
}