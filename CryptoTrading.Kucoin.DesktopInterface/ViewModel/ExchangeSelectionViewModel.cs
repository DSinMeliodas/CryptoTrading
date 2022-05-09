using CryptoTrading.Kucoin.DesktopInterface.Adapters;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : UpdatingViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";

    private readonly IExchangeManager m_ExchangeManager = new ExchangeManager();

    private IEnumerable<ISeries> m_CurrentSeries;
    private Visibility m_CurrentVisibility = Visibility.Hidden;
    private Axis[] m_CurrentXAxis;
    private ObservableCollection<string> m_Exchanges;
    private ObservableCollection<Exchange> m_OpenedExchanges;
    private int m_SelectedIndex = DefaultIndex;
    private int m_SelectedOpenedIndex = DefaultIndex;

    public IEnumerable<ISeries> CurrentSeries
    {
        get => m_CurrentSeries ??= CreateDefaultSeries();
        set
        {
            m_CurrentSeries = value;
            OnPropertyChanged();
        }
    }

    public Axis[] CurrentXAxis
    {
        get => m_CurrentXAxis ??= CreateDefaultAxis();
        set
        {
            m_CurrentXAxis = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Exchanges
    {
        get => m_Exchanges ??= new();
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

            Exchange exchange;
            try
            {
                exchange = LoadExchange();
            }
            catch
            {
                //catching call exceptions in order to prevent crashs silently
                return;
            }
            SelectExchange(exchange);
            // Lade Daten für gewählten exchange
            // aus applikations sicht ist es eine query
            // hier brauche ich keine UseCases die kommen aus dem Model
        }
    }

    public int SelectedOpenedIndex
    {
        get => m_SelectedOpenedIndex;
        set
        {
            m_SelectedOpenedIndex = value;
            OnPropertyChanged();
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

    public Visibility CurrentVisibility
    {
        get => m_CurrentVisibility;
        set
        {
            m_CurrentVisibility = value;
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
        var index = Exchanges.IndexOf(DefaultExchange);
        SelectedIndex = index == -1 && Exchanges.Any() ? 0 : index;
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
        CurrentVisibility = Visibility.Hidden;
        OpenedExchanges = new ObservableCollection<Exchange>(args.OpenedExchanges);
        if ((args.Action & ChangeAction.Seleced) != ChangeAction.Seleced)
        {
            return;
        }
        SelectedOpenedIndex = args.CurrentIndex;
        CurrentSeries = new ObservableCollection<ISeries>()
        {
            new CandlesticksSeries<FinancialPoint>()
            {
                Values = new ObservableCollection<FinancialPoint>(OpenedExchanges[SelectedOpenedIndex].Course.Select(new ExchangeCandleConverter().ConvertFrom))
            }
        };

    }

    private IEnumerable<ISeries> CreateDefaultSeries()
    {
        return new ObservableCollection<ISeries>()
        {
            new CandlesticksSeries<FinancialPoint>()
            {
                Values = new ObservableCollection<FinancialPoint>()
            }
        };
    }

    private Axis[] CreateDefaultAxis()
    {
        return new Axis[]
        {
            new ()
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long)value).ToString("hh:mm dd.MM.yyyy"),
                UnitWidth = TimeSpan.FromMinutes(1).Ticks,
                Name = "Zeit",
                ShowSeparatorLines = true
            }
        };
    }
}
