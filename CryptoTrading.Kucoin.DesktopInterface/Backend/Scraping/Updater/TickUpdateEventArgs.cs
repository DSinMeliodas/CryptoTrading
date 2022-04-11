using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

using System;
using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public delegate void OnTickUpdate(ITickUpdater sender, TickUpdateEventArgs args);

public class TickUpdateEventArgs : EventArgs
{

    private readonly IReadOnlyDictionary<TickUpdateSubscription, object> m_Subscriptions;

    public TickUpdateEventArgs(IReadOnlyDictionary<TickUpdateSubscription, object> subscription)
    {
        m_Subscriptions = subscription;
    }

    public bool TryGetSubscriptionResult<T>(TickUpdateSubscription subscription, out T subscriptionValue)
    {
        var success = m_Subscriptions.TryGetValue(subscription,  out var result);
        subscriptionValue = default;
        return success && subscription.TryCastSubscribed(result, out subscriptionValue);
    }
}