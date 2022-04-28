
using CryptoTrading.Kucoin.DesktopInterface.Backend.Extensions;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using Kucoin.Net.Objects.Models.Spot;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

internal delegate void ExchangesChanged(IReadOnlyCollection<string> currentExchanges);

internal sealed class ExchangesUpdate : SubscriptionCallBackBase
{
    public event EventHandler<ExchangesChangedEventArgs> OnExchangesChanged;

    private readonly List<string> m_Exchanges = new();


    public override void OnTickUpdate(TickUpdateEventArgs args)
    {
        if (!CheckArgs(args, out IEnumerable<KucoinSymbol> subscription))
        {
            return;
        }

        var exchangeNames = subscription.Select(symbol => symbol.Symbol).ToHashSet();
        var contained = m_Exchanges.Intersect(exchangeNames).ToHashSet();
        if (contained.Count == m_Exchanges.Count)
        {
            m_Exchanges.AddDifferenceToCollection(exchangeNames, contained);
            OnExchangesChanged?.Invoke(this, new(m_Exchanges));
            return;
        }
        var notPresentAnyMore = m_Exchanges.Except(exchangeNames).ToHashSet();
        m_Exchanges.Clear();
        m_Exchanges.AddRange(exchangeNames);
        OnExchangesChanged?.Invoke(this, new (m_Exchanges, notPresentAnyMore));
    }
}