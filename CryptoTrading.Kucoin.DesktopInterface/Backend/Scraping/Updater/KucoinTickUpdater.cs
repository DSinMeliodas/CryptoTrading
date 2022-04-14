
using CryptoTrading.Kucoin.DesktopInterface.Backend.Querying;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

using Kucoin.Net.Clients;

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal sealed class KucoinTickUpdater : ITickUpdater
{
    private readonly ConcurrentDictionary<TickUpdateSubscription, bool> m_Subscriptions = new();
    private readonly KucoinClient m_Client = new();
    private readonly Timer m_Ticker;

    private TimeSpan m_UpdateInterval = ITickUpdater.DefaultUpdateInterval;
    private bool m_Running;

    public TimeSpan UpdateInterval
    {
        get => m_UpdateInterval;
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            var oldValue = m_UpdateInterval;
            m_UpdateInterval = value;
            if (!m_Running)
            {
                return;
            }
            if (m_Ticker.Change(TimeSpan.Zero, UpdateInterval))
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

    public bool Start()
    {
        if (m_Running)
        {
            return false;
        }

        m_Running = true;
        return m_Ticker.Change(TimeSpan.Zero, UpdateInterval);
    }

    public bool Stop()
    {
        if (!m_Running)
        {
            return false;
        }

        m_Running = false;
        return m_Ticker.Change(Timeout.InfiniteTimeSpan, UpdateInterval);
    }

    public TickUpdateSubscription Subscribe(ITickerTarget target, ISubscriptionCallBack callBack)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(callBack);
        var id = Guid.NewGuid();
        var subscription = new TickUpdateSubscription(id, target, callBack, target.ResultType);
        if (!m_Subscriptions.TryAdd(subscription, false))
        {
            throw new ArgumentException("target already subscribed");
        }
        return subscription;

    }

    public void Unsubscribe(TickUpdateSubscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);
        _ = m_Subscriptions.TryRemove(subscription, out _);
    }

    private void OnTick(object _)
    {
        var resultsBySubscription = SubscriptionQuery.All(m_Subscriptions.Keys)
                                .UpdateOn(m_Client)
                                .RemapInnerValueToOuterValue();
        foreach (var subscriptionWithResult in resultsBySubscription)
        {
            var subscription = subscriptionWithResult.Key;
            var asyncResult = subscriptionWithResult.Value;
            subscription.NotifyTickUpdate(asyncResult);
        }
    }
}