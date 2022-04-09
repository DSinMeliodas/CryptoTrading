using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private readonly ExchangesWatcherCallBack m_CallBack;

    private readonly Dictionary<string, Exchange> m_SelectedExchangeMapping = new();
    private ObservableCollection<string> m_Exchanges;

    public ObservableCollection<string> Exchanges
    {
        get => m_Exchanges??= new();
        private set
        {
            m_Exchanges = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Exchange> SelectedExchanges { get; } = new();

    public ExchangeSelectionViewModel() 
        : base(new InplaceUpdaterTarget(new ExchangeSymbols()),
            new ExchangesWatcherCallBack())
    { 
        m_CallBack = (ExchangesWatcherCallBack)Subscription.CallBack;
        m_CallBack.OnExchangesChanged += UpdateExchanges;
    }

    private void UpdateExchanges(object? sender, ExchangesChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }
    }

    private void RemoveWatchedExchanges(ICollection<string> toBeRemoved)
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
