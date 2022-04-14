using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using System;
using System.Collections.Generic;
using System.Threading;
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
    private readonly AutoResetEvent m_SubscribtionWaiter = new (false);
    
    private TimeSpan m_UpdateInterval = ITickUpdater.DefaultUpdateInterval;
    private bool m_IsRunning;

    public TimeSpan UpdateInterval
    {
        get => m_UpdateInterval;
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
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
            updater.Dispose();
        }

        foreach (var updater in m_InactiveUpdaters.Values)
        {
            updater.Dispose();
        }
        m_SubscribtionWaiter.Dispose();
    }

    public bool ManageUpdater(ITickUpdater updater, string identifier = InPlaceInstanceId)
    {
        updater.UpdateInterval = UpdateInterval;
        if (!m_IsRunning)
        {
            m_InactiveUpdaters.Add(identifier, updater);
            return true;
        }
        m_ActiveUpdaters.Add(identifier, updater);
        return updater.Start();
    }

    public bool Start(bool waitForSubscription)
    {
        if (waitForSubscription)
        {
            _ = Task.Run(() =>
            {
                _ = m_SubscribtionWaiter.WaitOne();
                _ = Start(false);
            });
            return true;
        }
        if (m_IsRunning)
        {
            return false;
        }
        m_IsRunning = true;
        foreach (var identifier in m_InactiveUpdaters.Keys)
        {
            if (!m_InactiveUpdaters.Remove(identifier, out var updater))
            {
                m_IsRunning = false;
            }
            m_ActiveUpdaters.Add(identifier, updater);
            m_IsRunning &= updater!.Start();
        }
        return m_IsRunning;
    }

    public bool Stop()
    {
        if (!m_IsRunning)
        {
            return false;
        }
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
        m_IsRunning = !success;
        return success;
    }

    public TickUpdateSubscription Subscribe(ITickUpdaterTarget target, ISubscriptionCallBack callBack)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(callBack);
        if (m_ActiveUpdaters.TryGetValue(target.UpdaterId, out var updater))
        {
            return SubscribeInternal(target, callBack, updater);
        }
        if (!m_InactiveUpdaters.TryGetValue(target.UpdaterId, out updater))
        {
            throw new KeyNotFoundException($"could not find updater '{target.UpdaterId}'");
        }
        return SubscribeInternal(target, callBack, updater);
    }

    public bool TryStartUpdater(string identifier)
    {
        return m_InactiveUpdaters.TryGetValue(identifier, out var updater) && updater.Start();
    }

    public bool TryStopUpdater(string identifier)
    {
        return m_ActiveUpdaters.TryGetValue(identifier, out var updater) && updater.Stop();
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

    private TickUpdateSubscription SubscribeInternal(
        ITickUpdaterTarget target,
        ISubscriptionCallBack callBack,
        ITickUpdater updater)
    {
        var result =  updater.Subscribe(target, callBack);
        _ = m_SubscribtionWaiter.Set();
        return result;
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