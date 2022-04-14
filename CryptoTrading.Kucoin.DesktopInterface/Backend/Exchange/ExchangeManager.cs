using System;
using CryptoTrading.Kucoin.DesktopInterface.Domain;

using System.Collections.Generic;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using Kucoin.Net.Objects.Models.Spot;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Exchange;

internal sealed class ExchangeManager : IExchangeManager
{

    public static ExchangeManager Instance { get; } = new ();

    private readonly Dictionary<ExchangeIdentifier, TickUpdateSubscription> m_ExchangeSubscriptionMapping = new ();

    public ExchangeIdentifier CurrentExchangeIdentifier { get; private set; }
    public IReadOnlySet<ExchangeIdentifier> OpenExchangeIdentifiers { get; } = new HashSet<ExchangeIdentifier>();

    private ExchangeManager()
    {
    }

    public void SetOpenedAsCurrent(ExchangeIdentifier exchangeIdentifier)
    {
        if (!OpenExchangeIdentifiers.Contains(exchangeIdentifier))
        {
            throw new ArgumentException("exchange is not already open");
        }

        StopUpdatingFor(CurrentExchangeIdentifier);
        CurrentExchangeIdentifier = exchangeIdentifier;
        StartUpdatingFor(CurrentExchangeIdentifier);
    }

    public void OpenExchange(ExchangeIdentifier exchangeIdentifier)
    {
        var target = new ExchangeTickUpdaterTarget(exchangeIdentifier);
        DataHub.Instance.ManageUpdater(new KucoinTickUpdater(), target.UpdaterId);
        DataHub.Instance.Subscribe(target, null);
    }

    private void StopUpdatingFor(ExchangeIdentifier exchangeIdentifier)
    {
        var subscription = m_ExchangeSubscriptionMapping[exchangeIdentifier];
        var target = (ExchangeTickUpdaterTarget)subscription.Target;
        _ = DataHub.Instance.TryStopUpdater(target.UpdaterId);
    }

    private void StartUpdatingFor(ExchangeIdentifier exchangeIdentifier)
    {
        var subscription = m_ExchangeSubscriptionMapping[exchangeIdentifier];
        var target = (ExchangeTickUpdaterTarget)subscription.Target;
        _ = DataHub.Instance.TryStartUpdater(target.UpdaterId);
    }
}