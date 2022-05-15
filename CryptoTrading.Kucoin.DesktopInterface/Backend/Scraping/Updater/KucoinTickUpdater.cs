using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Util;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal sealed class KucoinTickUpdater : ITickUpdater
{
    private readonly ManualResetEvent m_SubscriptionLock = new (true);
    private readonly HashSet<TickUpdateSubscription> m_Subscriptions = new();
    private readonly Timer m_Ticker;

    private TimeSpan m_UpdateInterval = ITickUpdater.DefaultUpdateInterval;

    public IExchangeUpdater BaseUpdater { get; } = new KucoinUpdater();

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
            if (!Running)
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
    public bool Running { get; private set; }

    public KucoinTickUpdater()
    {
        m_Ticker = new(OnTick);
    }

    public void Dispose()
    {
    }

    public bool Start()
    {
        if (Running)
        {
            return false;
        }

        Running = true;
        return m_Ticker.Change(TimeSpan.Zero, UpdateInterval);
    }

    public bool Stop()
    {
        if (!Running)
        {
            return false;
        }

        Running = false;
        return m_Ticker.Change(Timeout.InfiniteTimeSpan, UpdateInterval);
    }

    public TickUpdateSubscription Subscribe(IExchangeTarget target, ISubscriptionCallBack callBack)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(callBack);
        ThrowHelper.ThrowIfUndefined(target.DataTargetIdentifier);
        var id = Guid.NewGuid();
        var subscription = new TickUpdateSubscription(id, target, callBack);
        _ = m_SubscriptionLock.WaitOne();
        if (!m_Subscriptions.Add(subscription))
        {
            throw new ArgumentException("target already subscribed");
        }
        _ = m_SubscriptionLock.Set();
        return subscription;

    }

    public void Unsubscribe(TickUpdateSubscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);
        _ = m_SubscriptionLock.WaitOne();
        if (!m_Subscriptions.Remove(subscription))
        {
            throw new KeyNotFoundException($"subscription {subscription.Id} was not found");
        }
        _ = m_SubscriptionLock.Set();
    }

    private async void OnTick(object _)
    {
        _ = m_SubscriptionLock.WaitOne();
        foreach (var subscription in m_Subscriptions)
        {
            await Task.Delay(MapDelay(subscription.Target.DataTargetIdentifier));
            var callResult = BaseUpdater.MakeUpdateCall(subscription.Target);
            await subscription.NotifyTickUpdate(callResult);
        }
        _ = m_SubscriptionLock.Set();
    }

    private TimeSpan MapDelay(DataTargetIdentifier dataTargetIdentifier)
    {
        return dataTargetIdentifier switch
        {
            DataTargetIdentifier.ExchangeSymbols => TimeSpan.FromMilliseconds(400),
            DataTargetIdentifier.Exchange => TimeSpan.FromSeconds(15),
            _ => TimeSpan.Zero
        };
    }
}