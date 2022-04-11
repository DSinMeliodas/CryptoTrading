using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Extensions;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using Kucoin.Net.Objects.Models.Spot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

internal delegate void ExchangesChanged(IReadOnlyCollection<string> currentExchanges);

internal sealed class ExchangesWatcherCallBack : ISubscriptionCallBack
{
    public event EventHandler<ExchangesChangedEventArgs> OnExchangesChanged;

    private readonly List<string> m_Exchanges = new();

    public TickUpdateSubscription Subscription { get; set; }

    public void OnAsyncCallError(Task<CallResult<object>> obj)
    {
        throw new System.NotImplementedException();
    }

    public void OnCallError(CallResult<object> obj)
    {
        throw new System.NotImplementedException();
    }

    public void OnTickUpdate(ITickUpdater _, TickUpdateEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        if (!args.TryGetSubscriptionResult(Subscription, out IEnumerable<KucoinSymbol> subscription))
        {
            MessageBox.Show("Could not update Symbols.");
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

    private void OnExchangeCollection(ITickUpdater sender, TickUpdateEventArgs args)
    {
    }
}