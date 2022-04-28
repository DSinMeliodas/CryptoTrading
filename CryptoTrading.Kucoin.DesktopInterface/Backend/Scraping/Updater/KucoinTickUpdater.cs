using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

using Kucoin.Net.Clients;

using System;
using System.Collections.Generic;
using System.Threading;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal sealed class KucoinTickUpdater : ITickUpdater
{
    private readonly HashSet<TickUpdateSubscription> m_Subscriptions = new();
    private readonly Timer m_Ticker;

    private TimeSpan m_UpdateInterval = ITickUpdater.DefaultUpdateInterval;
    private bool m_Running;

    public IExchangeUpdater BaseUpdater { get; }

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

    public TickUpdateSubscription Subscribe(DataTargetIdentifier target, ISubscriptionCallBack callBack)
    {
        if (target is DataTargetIdentifier.Undefined)
        {
            throw new ArgumentException($"target is undefined", nameof(target));
        }
        ArgumentNullException.ThrowIfNull(callBack);
        var id = Guid.NewGuid();
        var subscription = new TickUpdateSubscription(id, target, callBack);
        if (!m_Subscriptions.Add(subscription))
        {
            throw new ArgumentException("target already subscribed");
        }
        return subscription;

    }

    public void Unsubscribe(TickUpdateSubscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);
        if (!m_Subscriptions.Remove(subscription))
        {
            throw new KeyNotFoundException($"subscription {subscription.Id} was not found");
        }
    }

    private async void OnTick(object _)
    {
        foreach (var subscription in m_Subscriptions)
        {
            var callResult = BaseUpdater.MakeUpdateCall(subscription.Target);
            await subscription.NotifyTickUpdate(callResult);
        }
    }
}