using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";

    private readonly Dictionary<string, Exchange> m_SelectedExchangeMapping = new();
    private ObservableCollection<string> m_Exchanges;
    private int m_SelectedIndex = -1;

    public ObservableCollection<string> Exchanges
    {
        get => m_Exchanges??= new();
        private set
        {
            m_Exchanges = value;
            OnPropertyChanged();
        }
    }

    public int SelectedIndex
    {
        get => m_SelectedIndex;
        set
        {
            m_SelectedIndex = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Exchange> SelectedExchanges { get; } = new();

    public ExchangeSelectionViewModel() 
        : base(new InplaceUpdaterTarget(new ExchangeSymbols()),
            new ExchangesWatcherCallBack())
    {
    }

    protected override void Init()
    {
        base.Init();
        var callBack = (ExchangesWatcherCallBack)Subscription.CallBack;
        callBack.OnExchangesChanged += UpdateExchanges;
    }

    private void UpdateExchanges(object? _, ExchangesChangedEventArgs e)
    {
        Exchanges = new ObservableCollection<string>(e.Exchanges.ToImmutableSortedSet());
        if (SelectedIndex == DefaultIndex)
        {
            SelectedIndex = Exchanges.IndexOf(DefaultExchange);
        }
        if (e.RemovedExchanges is null)
        {
            return;
        }
        RemoveWatchedExchanges(e.RemovedExchanges);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }
    }

    private void RemoveWatchedExchanges(IReadOnlyCollection<string> toBeRemoved)
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
