using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories;

internal sealed class KucoinExchangeRepository : IExchangeRepository
{
    private static KucoinExchangeRepository? s_SingletonInstance;
    public static KucoinExchangeRepository SingletonInstance => s_SingletonInstance ??= new KucoinExchangeRepository(KucoinUpdateIntervalSettings.Instance);

    private TickUpdateSubscription? m_ExchangeSymbolsSubscription;
    private readonly Dictionary<ExchangeSymbol, Exchange> m_Exchanges = new();
    private readonly Dictionary<ExchangeSymbol, List<IExchangeUpdateCallBack>> m_RegisteredCallBacks = new();
    private readonly Dictionary<ExchangeSymbol, TickUpdateSubscription> m_UpdateSubscriptions = new();
    private readonly ITickUpdater m_KucoinTickUpdater = new KucoinTickUpdater();
    private readonly ManualResetEvent m_Lock = new(true);
    private readonly IUpdateIntervalSettings m_UpdateTickSettings;

    private KucoinExchangeRepository(IUpdateIntervalSettings updateTickSettings)
    {
        m_UpdateTickSettings = updateTickSettings;
        m_UpdateTickSettings.OnUpdateIntervalChanged += SetUpdateInterval;
        m_UpdateTickSettings.OnAutoUpdatedChanged += SetAutoUpdated;
    }

    private void SetAutoUpdated(object? sender, bool e)
    {
        if (m_KucoinTickUpdater.Running && e || !m_KucoinTickUpdater.Running && !e)
        {
            return;
        }

        if (e)
        {
            _ = m_KucoinTickUpdater.Start();
            return;
        }
        _ = m_KucoinTickUpdater.Stop();
    }

    public void Dispose()
    {
        _ = m_Lock.WaitOne();
        if (m_ExchangeSymbolsSubscription is not null)
        {
            m_KucoinTickUpdater.Unsubscribe(m_ExchangeSymbolsSubscription);
        }
        m_UpdateTickSettings.OnUpdateIntervalChanged -= SetUpdateInterval;
        m_KucoinTickUpdater?.Dispose();
        m_Lock?.Dispose();
    }

    public async Task<IReadOnlyList<string>> GetAvailableExchanges(IExchangeSymbolsUpdateCallBack callBack)
    {
        ArgumentNullException.ThrowIfNull(callBack);
        _ = m_Lock.WaitOne();
        if (m_ExchangeSymbolsSubscription is not null)
        {
            _ = m_Lock.Set();
            return await Task.FromException<IReadOnlyList<string>>(new NotSupportedException("already subscribed to the exchanges"));
        }
        var rawResult = await m_KucoinTickUpdater.BaseUpdater.GetExchangeSymbols();
        var result = rawResult.Select(symbol => symbol.Symbol).ToList();
        m_ExchangeSymbolsSubscription = m_KucoinTickUpdater.Subscribe(new ExchangeSymbols(), new ExchangeSymbolsUpdate(callBack));
        _ = m_Lock.Set();
        return result;
    }

    public async Task<Exchange> GetExchange(ExchangeSymbol symbol, IExchangeUpdateCallBack callBack)
    {
        ArgumentNullException.ThrowIfNull(symbol);
        ArgumentNullException.ThrowIfNull(callBack);
        _ = m_Lock.WaitOne();
        if (m_Exchanges.TryGetValue(symbol, out var exchange))
        {
            m_RegisteredCallBacks[symbol].Add(callBack);
            return exchange;
        }
        var result = await RegisterAndLoadInitial(symbol, callBack);
        _ = m_Lock.Set();
        return result;
    }

    public Task DeleteExchangeSymbols()
    {
        _ = m_Lock.WaitOne();
        if (m_ExchangeSymbolsSubscription is null)
        {
            _ = m_Lock.Set();
            return Task.CompletedTask;
        }
        m_KucoinTickUpdater.Unsubscribe(m_ExchangeSymbolsSubscription);
        m_ExchangeSymbolsSubscription = null;
        _ = m_Lock.Set();
        return Task.CompletedTask;
    }

    public Task DeleteExchange(ExchangeSymbol symbol)
    {
        ArgumentNullException.ThrowIfNull(symbol);
        _ = m_Lock.WaitOne();
        if (!m_Exchanges.Remove(symbol))
        {
            return Task.CompletedTask;
        }

        _ = m_Exchanges.Remove(symbol);
        _ = m_UpdateSubscriptions.Remove(symbol, out var subscription);
        _ = m_RegisteredCallBacks.Remove(symbol);
        m_KucoinTickUpdater.Unsubscribe(subscription);
        if (m_Exchanges.Count == 0 && m_KucoinTickUpdater.Running)
        {
            _ = m_KucoinTickUpdater.Stop();
        }
        _ = m_Lock.Set();
        return Task.CompletedTask;
    }

    private async Task<Exchange> RegisterAndLoadInitial(ExchangeSymbol exchangeId, IExchangeUpdateCallBack callBack)
    {
        var callBacks = new List<IExchangeUpdateCallBack> { callBack };
        var subscription = m_KucoinTickUpdater.Subscribe(new ExchangeAutoUpdate(exchangeId), new ExchangeUpdate(callBacks));
        m_RegisteredCallBacks[exchangeId] = callBacks;
        m_UpdateSubscriptions[exchangeId] = subscription;
        var result = await m_KucoinTickUpdater.BaseUpdater.GetExchange(exchangeId);
        if (m_UpdateTickSettings.IsAutoUpdated && !m_KucoinTickUpdater.Running)
        {
            _ = m_KucoinTickUpdater.Start();
        }
        m_Exchanges.Add(exchangeId, result);
        return result;
    }

    private void SetUpdateInterval(object? _, TimeSpan newInterval)
    {
        m_KucoinTickUpdater.UpdateInterval = newInterval;
    }

    public static void Close()
    {
        s_SingletonInstance?.Dispose();
        s_SingletonInstance = null;
    }
}