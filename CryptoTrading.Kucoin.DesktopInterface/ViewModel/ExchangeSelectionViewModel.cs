
using CryptoTrading.Kucoin.DesktopInterface.Annotations;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Extensions;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.View;

using Kucoin.Net.Objects.Models.Spot;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private readonly TickUpdateSubscription m_Subscription;
    private readonly Dictionary<string, ExchangeTab> m_SelectedExchangeMapping = new();
    private ObservableCollection<string> m_Exchanges;
    private ObservableCollection<ExchangeTab> m_WatchedExchanges;

    public ObservableCollection<string> Exchanges
    {
        get => m_Exchanges??= new();
        private set
        {
            m_Exchanges= value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ExchangeTab> SelectedExchanges { get; } = new();

    public ExchangeSelectionViewModel()
    {
        m_Subscription = Updater.Subscribe(new ExchangeSymbols());
        Updater.OnTickUpdate += OnExchangeCollection;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }

        Updater.Unsubscribe(m_Subscription);
        Updater.OnTickUpdate -= OnExchangeCollection;
    }

    private void OnExchangeCollection(ITickUpdater sender, TickUpdateEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        if (!args.TryGetSubscriptionResult(m_Subscription, out IEnumerable<KucoinSymbol> subscription))
        {
            MessageBox.Show("Could not update Symbols.");
            return;
        }

        var exchangeNames = subscription.Select(symbol => symbol.Symbol).ToHashSet();
        var contained = Exchanges.Intersect(exchangeNames).ToHashSet();
        if (contained.Count == Exchanges.Count)
        {
            Exchanges.AddDifferenceToCollection(exchangeNames, contained);
            return;
        }
        var toBeRemoved = Exchanges.Except(exchangeNames).ToHashSet();
        Exchanges.Clear();
        Exchanges = new(exchangeNames);
        RemoveWatchedExchanges(toBeRemoved);
    }

    private void RemoveWatchedExchanges([NotNull]ICollection<string> toBeRemoved)
    {
        ArgumentNullException.ThrowIfNull(toBeRemoved);
        foreach (var exchange in toBeRemoved)
        {
            if (!m_SelectedExchangeMapping.Remove(exchange, out var removedTab))
            {
                _ = MessageBox.Show($"Trying to remove exchange '{exchange} failed, it was not present.");
                continue;
            }
            _ = SelectedExchanges.Remove(removedTab);
        }
    }
}
