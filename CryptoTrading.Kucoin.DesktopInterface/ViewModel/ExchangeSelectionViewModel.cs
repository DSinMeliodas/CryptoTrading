using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";
    
    private ObservableCollection<string> m_Exchanges;
    private int m_SelectedIndex = DefaultIndex;

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
        : base(new ExchangesUpdate())
    {
    }

    protected override void Init()
    {
        base.Init();
        var callBack = (ExchangesUpdate)Subscription.CallBack;
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
        var loadUseCase = new LoadExchangeUseCase(KucoinExchangeRepository.SingletonInstance);
        var callBack = new ExchangeLoadedCallback();
        callBack.OnLoaded += AddExchange;
        var request = new ExchangeRequest(Exchanges[SelectedIndex], callBack);
        var exchange = loadUseCase.Execute(request);
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
