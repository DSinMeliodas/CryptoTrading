using System.Collections.Generic;
using System.Linq;
using CryptoExchange.Net.Objects;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using Kucoin.Net.Clients;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Querying;

internal class SubscriptionQuery
{
    private readonly IEnumerable<TickUpdateSubscription> m_Subscriptions;

    private SubscriptionQuery(IEnumerable<TickUpdateSubscription> subscriptions)
    {
        m_Subscriptions = subscriptions;
    }

    public TaskQuery<WebCallResult> UpdateOn(KucoinClient client)
    {
        var groupedByTarget = m_Subscriptions.GroupBy(s => s.Target);
        var updateOn = groupedByTarget.Select(group => group.Key.UpdateOn(client));
        return TaskQuery<WebCallResult>.ForAll(updateOn);
    }


    public static SubscriptionQuery All(IEnumerable<TickUpdateSubscription> subscriptions)
    {
        return new SubscriptionQuery(subscriptions);
    }
}