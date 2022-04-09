﻿using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;



internal sealed class DataHub
{
    public const string InPlaceInstanceId = "Inplace";

    public static DataHub Instance => s_HubInstance ??= new DataHub();
    private static DataHub s_HubInstance;

    public event Action<string, Task<CallResult<object>>> OnAsyncCallError;

    public event Action<string, CallResult<object>> OnCallError;

    public event OnTickUpdate OnTickUpdate;

    private readonly Dictionary<string, ITickUpdater> m_ActiveUpdaters = new();
    private readonly Dictionary<string, ITickUpdater> m_InactiveUpdaters = new();
    private readonly Dictionary<TickUpdateSubscription, ITickUpdater> m_SubscriptionMapping = new();
    
    private TimeSpan m_UpdateInterval;

    public TimeSpan UpdateInterval
    {
        get => m_UpdateInterval;
        set
        {
            m_UpdateInterval = value;
            UpdateAllIntervals();
        }
    }

    private DataHub()
    {
    }

    public void Dispose()
    {
        foreach (var updater in m_ActiveUpdaters.Values)
        {
            updater?.Dispose();
        }

        foreach (var updater in m_InactiveUpdaters.Values)
        {
            updater?.Dispose();
        }
    }

    public bool ManageUpdater(ITickUpdater updater, string identifier = InPlaceInstanceId)
    {
        m_ActiveUpdaters.Add(identifier, updater);
        updater.UpdateInterval = UpdateInterval;
        return updater.Start();
    }

    public bool Start()
    {
        var success = true;
        foreach (var identifier in m_InactiveUpdaters.Keys)
        {
            if (!m_InactiveUpdaters.Remove(identifier, out var updater))
            {
                success = false;
            }
            m_ActiveUpdaters.Add(identifier, updater);
            success &= updater!.Start();
        }
        return success;
    }

    public bool Stop()
    {
        var success = true;
        foreach (var identifier in m_InactiveUpdaters.Keys)
        {
            if (!m_InactiveUpdaters.Remove(identifier, out var updater))
            {
                success = false;
            }
            m_ActiveUpdaters.Add(identifier, updater);
            success &= updater!.Stop();
        }
        return success;
    }

    public TickUpdateSubscription Subscribe(ITickUpdaterTarget target, ISubscriptionCallBack callBack)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(callBack);
        if (m_ActiveUpdaters.TryGetValue(target.UpdaterId, out var updater))
        {
            return updater.Subscribe(target, callBack);
        }
        if (!m_InactiveUpdaters.TryGetValue(target.UpdaterId, out updater))
        {
            throw new KeyNotFoundException($"could not find updater '{target.UpdaterId}'");
        }
        return updater.Subscribe(target, callBack);
    }

    public void Unsubscribe(TickUpdateSubscription subscription)
    {
        if (!m_SubscriptionMapping.TryGetValue(subscription, out var updater))
        {
            return;
        }
        _ = m_SubscriptionMapping.Remove(subscription);
        updater.Unsubscribe(subscription);
    }

    private void UpdateAllIntervals()
    {
        foreach (var updater in m_ActiveUpdaters.Values)
        {
            updater.UpdateInterval = UpdateInterval;
        }
        foreach (var updater in m_InactiveUpdaters.Values)
        {
            updater.UpdateInterval = UpdateInterval;
        }
    }

    public static void ShutDownAll()
    {
        _ = s_HubInstance.Stop();
        s_HubInstance?.Dispose();
        s_HubInstance = null;
    }
}