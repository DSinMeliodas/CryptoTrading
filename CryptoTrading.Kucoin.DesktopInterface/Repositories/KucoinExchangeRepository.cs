using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

internal sealed class KucoinExchangeRepository : IExchangeRepository
{
    private static KucoinExchangeRepository? s_SingletonInstance;
    public static KucoinExchangeRepository SingletonInstance => s_SingletonInstance ??= new KucoinExchangeRepository();

    private readonly Dictionary<ExchangeIdentifier, Exchange> m_Exchanges = new ();
    private readonly Dictionary<ExchangeIdentifier, List<IExchangeUpdateCallBack>> m_RegisteredCallBacks = new();
    private readonly Dictionary<ExchangeIdentifier, TickUpdateSubscription> m_UpdateSubscriptions = new ();
    private readonly ITickUpdater m_KucoinTickUpdater = new KucoinTickUpdater();
    private readonly ManualResetEvent m_Lock = new (true);

    private KucoinExchangeRepository()
    {
    }

    public void Dispose()
    {
        _ = m_Lock.WaitOne();
        m_KucoinTickUpdater?.Dispose();
        m_Lock?.Dispose();
    }

    public async Task<Exchange> GetExchange(ExchangeIdentifier exchangeId, IExchangeUpdateCallBack callBack)
    {
        _ = m_Lock.WaitOne();
        if (m_Exchanges.TryGetValue(exchangeId, out var exchange))
        {
            m_RegisteredCallBacks[exchangeId].Add(callBack);
            return exchange;
        }
        var result =  await RegisterAndLoadInitial(exchangeId, callBack);
        _ = m_Lock.Set();
        return result;
    }

    public Task DeleteExchange(ExchangeIdentifier exchangeId)
    {
        _ = m_Lock.WaitOne();
        if (!m_Exchanges.Remove(exchangeId))
        {
            return Task.CompletedTask;
        }

        _ = m_Exchanges.Remove(exchangeId);
        _ = m_UpdateSubscriptions.Remove(exchangeId, out var subscription);
        _ = m_RegisteredCallBacks.Remove(exchangeId);
        m_KucoinTickUpdater.Unsubscribe(subscription);
        if (m_Exchanges.Count == 0 && m_KucoinTickUpdater.Running)
        {
            _ = m_KucoinTickUpdater.Stop();
        }
        _ = m_Lock.Set();
        return Task.CompletedTask;
    }

    private async Task<Exchange> RegisterAndLoadInitial(ExchangeIdentifier exchangeId, IExchangeUpdateCallBack callBack)
    {
        var callBacks = new List<IExchangeUpdateCallBack> { callBack };
        var subscription = m_KucoinTickUpdater.Subscribe(new ExchangeAutoUpdate(exchangeId), new ExchangeUpdate(callBacks));
        m_RegisteredCallBacks[exchangeId] = callBacks;
        m_UpdateSubscriptions[exchangeId] = subscription;
        var result = await m_KucoinTickUpdater.BaseUpdater.GetExchange(exchangeId);
        if (!m_KucoinTickUpdater.Running)
        {
            _ = m_KucoinTickUpdater.Start();
        }
        return result;
    }

    public static void Close()
    {
        s_SingletonInstance?.Dispose();
        s_SingletonInstance = null;
    }
}