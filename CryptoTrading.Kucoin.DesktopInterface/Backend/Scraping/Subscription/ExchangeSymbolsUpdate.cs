using CryptoTrading.Kucoin.DesktopInterface.Backend.Extensions;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using Kucoin.Net.Objects.Models.Spot;

using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

internal sealed class ExchangeSymbolsUpdate : SubscriptionCallBackBase
{
    private readonly IExchangeSymbolsUpdateCallBack m_CallBack;
    private readonly List<string> m_Exchanges = new();

    public ExchangeSymbolsUpdate(IExchangeSymbolsUpdateCallBack callBack)
    {
        m_CallBack = callBack;
    }

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
            m_CallBack.NotifySymbolsChanged(m_Exchanges.ToArray());
            return;
        }
        var notPresentAnyMore = m_Exchanges.Except(exchangeNames).ToHashSet();
        m_Exchanges.Clear();
        m_Exchanges.AddRange(exchangeNames);
        m_CallBack.NotifySymbolsChanged(m_Exchanges.ToArray(), notPresentAnyMore);
    }
}