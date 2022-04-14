using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Exchange;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";
    
    private ObservableCollection<string> m_Exchanges;
    private int m_SelectedIndex;
    private string m_SelectedExchange;

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

    public string SelectedExchange
    {
        get => m_SelectedExchange;
        set
        {
            m_SelectedExchange = value;
            OnPropertyChanged();
            var useCase = new SelectExchange(ExchangeManager.Instance, m_SelectedExchange);
            useCase.Execute();
        }
    }

    public ObservableCollection<Exchange> OpenedExchanges { get; private set; } = new ();


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
        var stillOpen = OpenedExchanges.Where(ex => toBeRemoved.Contains(ex.Identifier.Identifier));
        OpenedExchanges = new(stillOpen);
        
    }
}
