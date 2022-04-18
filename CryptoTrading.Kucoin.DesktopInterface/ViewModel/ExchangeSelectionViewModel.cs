using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";
    
    private ObservableCollection<string> m_Exchanges;
    private int m_SelectedIndex = DefaultIndex;
    private IExchangeRepository m_ExchangeRepository;

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
            if (m_SelectedIndex < 0)
            {
                return;
            }

            var selectedExchange = Exchanges[m_SelectedIndex];
            if (string.IsNullOrWhiteSpace(selectedExchange))
            {
                return;
            }

            LoadExchange();
            // Lade Daten für gewählten exchange
            // aus applikations sicht ist es eine query
            // hier brauche ich keine UseCases die kommen aus dem Model
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

    private void LoadExchange()
    {
        var loadUseCase = new LoadExchangeUseCase(m_ExchangeRepository);
        var exchange = loadUseCase.Execute(null);
        AddExchange(exchange);
    }

    private void AddExchange(Exchange exchange)
    {
        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            OpenedExchanges.Add(exchange);
        });
    }

    private void RemoveWatchedExchanges(IReadOnlyCollection<string> toBeRemoved)
    {
        ArgumentNullException.ThrowIfNull(toBeRemoved);
        var stillOpen = OpenedExchanges.Where(ex => toBeRemoved.Contains(ex.Identifier.Symbol));
        OpenedExchanges = new(stillOpen);
        
    }
}
