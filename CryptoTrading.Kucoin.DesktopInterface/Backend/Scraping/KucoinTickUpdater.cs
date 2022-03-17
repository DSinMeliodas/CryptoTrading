using Kucoin.Net.Clients;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

internal sealed class KucoinTickUpdater : ITickUpdater
{
    public event Action OnRequestTimeout;

    public event OnTickUpdate OnTickUpdate;

    private readonly ConcurrentBag<TickUpdateSubscription> m_Subscriptions = new();
    private readonly KucoinClient m_Client = new ();
    private readonly Timer m_Ticker;

    private TimeSpan m_UpdateInterval;

    public TimeSpan UpdateInterval
    {
        get=>m_UpdateInterval;
        set
        {
            var oldValue = m_UpdateInterval;
            m_UpdateInterval = value;
            if (Start())
            {
                return;
            }
            m_UpdateInterval = oldValue;
        }
    }

    public KucoinTickUpdater()
    {
        m_Ticker = new(OnTick);
    }

    public void Dispose()
    {
        m_Ticker?.Dispose();
        m_Client?.Dispose();
    }

    public bool Start() => m_Ticker.Change(TimeSpan.Zero, UpdateInterval);

    public bool Stop() => m_Ticker.Change(Timeout.InfiniteTimeSpan, UpdateInterval);

    public TickUpdateSubscription Subscribe(string target, Type targetType)
    {
        var id = Guid.NewGuid();
        var subscription = new TickUpdateSubscription(id, target, targetType);
        m_Subscriptions.Add(subscription);
        return subscription;

    }

    private void OnTick(object _)
    {
        foreach (var subscription in m_Subscriptions)
        {
            subscription.Target
        }
    }
}