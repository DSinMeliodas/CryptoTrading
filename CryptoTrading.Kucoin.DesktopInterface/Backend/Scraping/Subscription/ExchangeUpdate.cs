using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

internal sealed class ExchangeUpdate : SubscriptionCallBackBase
{
    private readonly IReadOnlyList<IExchangeUpdateCallBack> m_CallBacks;

    public ExchangeUpdate(IReadOnlyList<IExchangeUpdateCallBack> callBacks)
    {
        m_CallBacks = callBacks;
    }

    public override void OnTickUpdate(TickUpdateEventArgs args)
    {
        if (!CheckArgs(args, out Exchange exchange))
        {
            return;
        }
        foreach (var callBack in m_CallBacks)
        {
            callBack.OnExchangeUpdate(exchange);
        }
    }
}