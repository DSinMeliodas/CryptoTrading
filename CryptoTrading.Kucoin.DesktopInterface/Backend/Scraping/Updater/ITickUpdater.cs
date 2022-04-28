
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface ITickUpdater : IDisposable
{
    static TimeSpan DefaultUpdateInterval { get; } = TimeSpan.FromMinutes(1);

    TimeSpan UpdateInterval { get; set; }

    IExchangeUpdater BaseUpdater { get; }

    bool Start();

    bool Stop();

    TickUpdateSubscription Subscribe(DataTargetIdentifier target, ISubscriptionCallBack subscriptionCallBack);

    void Unsubscribe(TickUpdateSubscription subscription);
}