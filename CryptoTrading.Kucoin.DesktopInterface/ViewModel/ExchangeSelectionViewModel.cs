using System;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";

    private readonly IExchangeManager m_ExchangeManager = new ExchangeManager();

    private ObservableCollection<string> m_Exchanges;
    private ObservableCollection<Exchange> m_OpenedExchanges;
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

            var exchange = LoadExchange();
            SelectExchange(exchange);
            // Lade Daten für gewählten exchange
            // aus applikations sicht ist es eine query
            // hier brauche ich keine UseCases die kommen aus dem Model
        }
    }

    public ObservableCollection<Exchange> OpenedExchanges
    {
        get => m_OpenedExchanges ??= new();
        private set
        {
            m_OpenedExchanges = value;
            OnPropertyChanged();
        }
    }

    public ExchangeSelectionViewModel()
    {
        m_ExchangeManager.OnOpenedExchangesChanged += OnOpenExchangesChanged;
    }

    protected override void Init()
    {
        base.Init();
        var useCase = new LoadAvailableExchanges(KucoinExchangeRepository.SingletonInstance);
        var callBack = new LoadAvailableExchangesCallBack();
        callBack.OnSymbolsChanged += UpdateExchanges;
        Exchanges = new ObservableCollection<string>(useCase.Execute(callBack));
    }

    private void UpdateExchanges(object? _, ExchangeSymbolsChangedEventArgs args)
    {
        Exchanges = new ObservableCollection<string>(args.Exchanges.ToImmutableSortedSet());
        if (SelectedIndex == DefaultIndex)
        {
            SelectedIndex = Exchanges.IndexOf(DefaultExchange);
        }
        if (args.RemovedExchanges is null)
        {
            return;
        }
        var useCase = new RemoveExchanges(m_ExchangeManager);
        useCase.Execute(args.RemovedExchanges);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
        {
            return;
        }
        m_ExchangeManager?.Dispose();
    }

    private Exchange LoadExchange()
    {
        var loadUseCase = new LoadExchange(KucoinExchangeRepository.SingletonInstance);
        var callBack = new ExchangeLoadedCallback();
        callBack.OnLoaded += m_ExchangeManager.UpdateExchange;
        var request = new ExchangeRequest(Exchanges[SelectedIndex], callBack);
        return loadUseCase.Execute(request);

    }

    private void OnOpenExchangesChanged(object _, ExchangeChangedArgs args)
    {
        if (args.Action == ChangeAction.Undefined)
        {
            throw new ArgumentException("undefined action", nameof(args.Action));
        }
        InvokeOnDispatcher(SetOpenedExchanges, args);
    }

    private void SelectExchange(Exchange exchange)
    {
        var useCase = new SelectExchange(m_ExchangeManager);
        useCase.Execute(exchange);
    }

    private void SetOpenedExchanges(ExchangeChangedArgs args)
    {
        OpenedExchanges = new ObservableCollection<Exchange>(args.OpenedExchanges);
        if ((args.Action & ChangeAction.Seleced) != ChangeAction.Seleced)
        {
            return;
        }

        SelectedIndex = args.CurrentIndex;
    }
}
