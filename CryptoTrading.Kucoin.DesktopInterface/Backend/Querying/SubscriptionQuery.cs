using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

using Kucoin.Net.Clients;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Querying;

internal class SubscriptionQuery
{
    private readonly IEnumerable<TickUpdateSubscription> m_Subscriptions;

    private SubscriptionQuery(IEnumerable<TickUpdateSubscription> subscriptions)
    {
        m_Subscriptions = subscriptions;
    }

    public RemapQuery<ITickerTarget, TickUpdateSubscription, Task<CallResult<object>>> UpdateOn(KucoinClient client)
    {
        var groupedByTarget = m_Subscriptions.GroupBy(s => s.Target);
        var updateOn = groupedByTarget.ToDictionary(group=>group, group => group.Key.UpdateOn(client));
        return RemapQuery<ITickerTarget, TickUpdateSubscription, Task<CallResult<object>>>.ForAll(updateOn);
    }


    public static SubscriptionQuery All(IEnumerable<TickUpdateSubscription> subscriptions)
    {
        return new SubscriptionQuery(subscriptions);
    }
}