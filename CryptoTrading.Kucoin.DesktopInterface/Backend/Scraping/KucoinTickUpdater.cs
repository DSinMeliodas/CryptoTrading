using Kucoin.Net.Clients;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Querying;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

internal sealed class KucoinTickUpdater : ITickUpdater
{
    public event Action<Task<CallResult<object>>> OnAsyncCallError; 
    public event Action<CallResult<object>> OnCallError;
    public event OnTickUpdate OnTickUpdate;

    private readonly ConcurrentDictionary<TickUpdateSubscription, bool> m_Subscriptions = new();
    private readonly KucoinClient m_Client = new ();
    private readonly Timer m_Ticker;

    private TimeSpan m_UpdateInterval;

    public TimeSpan UpdateInterval
    {
        get => m_UpdateInterval;
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

    public TickUpdateSubscription Subscribe(ITickerTarget target, Type targetType)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(targetType);
        var id = Guid.NewGuid();
        var subscription = new TickUpdateSubscription(id, target, targetType);
        _ = m_Subscriptions.TryAdd(subscription, false);
        return subscription;

    }

    public void Unsubscribe(TickUpdateSubscription subscription)
    {
        ArgumentNullException.ThrowIfNull(subscription);
        _ = m_Subscriptions.TryRemove(subscription, out _);
    }

    private async void OnTick(object _)
    {
        var resultsBySubscription = SubscriptionQuery.All(m_Subscriptions.Keys)
                                .UpdateOn(m_Client)
                                .RemapInnerValueToOuterValue();
        var actualResultsBySubscription = resultsBySubscription.AsParallel().Select(ToResultOrErrorEventCall).Where(r=>r.Item3);
    }

    private (TickUpdateSubscription, object, bool) ToResultOrErrorEventCall(KeyValuePair<TickUpdateSubscription, Task<CallResult<object>>> kvp)
    {
        var task = kvp.Value;
        task.Wait();
        if (!task.IsCompletedSuccessfully)
        {
            OnAsyncCallError?.Invoke(kvp.Value);
            return (kvp.Key, null, false);
        }
        var call = task.Result;
        if (call.Success)
        {
            return (kvp.Key, call.Data, true);
        }
        OnCallError?.Invoke(call);
        return (kvp.Key, call.Data, false);

    }
}