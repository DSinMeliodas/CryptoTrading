using CryptoTrading.Kucoin.DesktopInterface.Adapters;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Commands;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;
using CryptoTrading.Kucoin.DesktopInterface.UseCases;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal sealed class ExchangeSelectionViewModel : DelaysedInitialisationViewModel
{
    private const int DefaultIndex = -1;
    private const string DefaultExchange = "BTC-USDT";
    private const string AutoUpdatedOnText = "Auto Update On";
    private const string AutoUpdatedOffText = "Auto Update Off";
    private readonly SKColor DefaultFontColor = SKColor.Parse("#e5e5e5");

    private readonly IExchangeManager m_ExchangeManager = new ExchangeManager();

    private Margin m_ChartMargin = new(100);
    private ObservableCollection<ISeries> m_CurrentSeries;
    private Visibility m_CurrentVisibility = Visibility.Hidden;
    private Axis[] m_XAxis;
    private Axis[] m_YAxis;
    private ObservableCollection<string> m_Exchanges;
    private ObservableCollection<Exchange> m_OpenedExchanges;
    private int m_SelectedExchangeIndex = DefaultIndex;
    private int m_SelectedOpenedIndex = DefaultIndex;
    private bool m_SelectedOpenedIndexFromEvent;
    private int m_SelectedUpdateIntervalIndex;
    private string m_AutoUpdateButtonText;

    public string AutoUpdateButtonText
    {
        get => m_AutoUpdateButtonText ??= UpdateSettings.IsAutoUpdated ? AutoUpdatedOnText : AutoUpdatedOffText;
        set
        {
            m_AutoUpdateButtonText = value;
            OnPropertyChanged();
        }
    }

    public ICommand AutoUpdateCommand { get; } = new SetAutoUpdateCommand();

    public Margin ChartMargin
    {
        get => m_ChartMargin;
        set
        {
            m_ChartMargin = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ISeries> CurrentSeries
    {
        get => m_CurrentSeries ??= CreateDefaultSeries();
        set
        {
            m_CurrentSeries = value;
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

    public ObservableCollection<string> Exchanges
    {
        get => m_Exchanges ??= new();
        private set
        {
            m_Exchanges = value;
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

    public int SelectedExchangeIndex
    {
        get => m_SelectedExchangeIndex;
        set
        {
            m_SelectedExchangeIndex = value;
            OnPropertyChanged();
            if (m_SelectedExchangeIndex < 0)
            {
                return;
            }

            var selectedExchange = Exchanges[m_SelectedExchangeIndex];
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
        }
    }

    public int SelectedOpenedIndex
    {
        get => m_SelectedOpenedIndex;
        set
        {
            m_SelectedOpenedIndex = value;
            OnPropertyChanged();
            if (m_SelectedOpenedIndexFromEvent)
            {
                return;
            }
            if (m_SelectedOpenedIndex < 0)
            {
                return;
            }
            m_ExchangeManager.SetOpenedAsCurrent(OpenedExchanges[SelectedOpenedIndex].Symbol);
        }
    }

    public int SelectedUpdateIntervalIndex
    {
        get => m_SelectedUpdateIntervalIndex;
        set
        {
            m_SelectedUpdateIntervalIndex = value;
            OnPropertyChanged();
            ChangeUpdateInterval();
        }
    }

    public IUpdateIntervalSettings UpdateSettings => KucoinUpdateIntervalSettings.Instance;

    public Axis[] XAxis
    {
        get => m_XAxis ??= CreateDefaultXAxis();
        set
        {
            m_XAxis = value;
            OnPropertyChanged();
        }
    }

    public Axis[] YAxis
    {
        get => m_YAxis ??=CreateDefaultYAxis();
        set
        {
            m_YAxis = value;
            OnPropertyChanged();
        }
    }

    public IReadOnlyList<UpdateInterval> UpdateIntervals => UpdateInterval.AllIntervals;

    public ExchangeSelectionViewModel()
    {
        m_ExchangeManager.OnOpenedExchangesChanged += OnOpenExchangesChanged;
        UpdateSettings.OnAutoUpdatedChanged += OnAutoUpdatedChanged;
    }

    protected override void Init()
    {
        base.Init();
        var useCase = new LoadAvailableExchangeSymbols(KucoinExchangeRepository.SingletonInstance);
        var callBack = new LoadAvailableExchangesCallBack();
        callBack.OnSymbolsChanged += UpdateExchanges;
        Exchanges = new ObservableCollection<string>(useCase.Execute(callBack));
        var index = Exchanges.IndexOf(DefaultExchange);
        SelectedExchangeIndex = index == -1 && Exchanges.Any() ? 0 : index;
        var contextBasedUseCaseCommand = AutoUpdateCommand as ContextBasedUseCaseCommand<IUpdateIntervalSettings>;
        contextBasedUseCaseCommand?.NotifyCanExecuteChanged();
    }

    private void UpdateExchanges(object? _, ExchangeSymbolsChangedEventArgs args)
    {
        Exchanges = new ObservableCollection<string>(args.Exchanges.ToImmutableSortedSet());
        if (SelectedExchangeIndex == DefaultIndex)
        {
            SelectedExchangeIndex = Exchanges.IndexOf(DefaultExchange);
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
        var request = new ExchangeRequest(Exchanges[SelectedExchangeIndex], callBack);
        return loadUseCase.Execute(request);
    }

    private void OnAutoUpdatedChanged(object? _, bool e)
    {
        AutoUpdateButtonText = e ? AutoUpdatedOnText : AutoUpdatedOffText;
    }

    private void OnOpenExchangesChanged(object? _, ExchangeChangedArgs args)
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
        m_SelectedOpenedIndexFromEvent = true;
        CurrentVisibility = Visibility.Hidden;
        OpenedExchanges = new ObservableCollection<Exchange>(args.OpenedExchanges);
        if ((args.Action & ChangeAction.Seleced) != ChangeAction.Seleced)
        {
            return;
        }
        //In order to prevent recursion we need to set this to true
        m_SelectedOpenedIndexFromEvent = true;
        SelectedOpenedIndex = args.CurrentIndex;
        var openedCourse = OpenedExchanges[SelectedOpenedIndex].History.Course;
        var financialPoints = openedCourse.Select(new FinancialCandleConverter().ConvertFrom);
        CurrentSeries.Clear();
        CurrentSeries.Add(new CandlesticksSeries<FinancialPoint> { Values = financialPoints });
        UpdateYAxisName();
        UpdateChartView();
        m_SelectedOpenedIndexFromEvent = false;
    }

    private ObservableCollection<ISeries> CreateDefaultSeries()
    {
        return new()
        {
            new CandlesticksSeries<FinancialPoint>()
            {
                Values = new ObservableCollection<FinancialPoint>(),
            },
            new LineSeries<decimal>()
            {
                Values = new ObservableCollection<decimal>()
            }
        };
    }

    private void ChangeUpdateInterval()
    {
        var useCase = new ChangeUpdateInterval(UpdateSettings);
        useCase.Execute(SelectedUpdateIntervalIndex);
    }

    private Axis[] CreateDefaultXAxis()
    {
        return new Axis[]
        {
            new ()
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long)value).ToString("hh:mm dd.MM.yyyy"),
                UnitWidth = TimeSpan.FromMinutes(1).Ticks,
                Name = "Zeit",
                ShowSeparatorLines = true,
                NamePaint = new SolidColorPaint{Color = DefaultFontColor},
                LabelsPaint = new SolidColorPaint{Color = DefaultFontColor}
            }
        };
    }

    private Axis[] CreateDefaultYAxis()
    {
        return new Axis[]
        {
            new ()
            {
                ShowSeparatorLines = true,
                NamePaint = new SolidColorPaint{Color = DefaultFontColor},
                LabelsPaint = new SolidColorPaint{Color = DefaultFontColor}
            }
        };
    }

    private void UpdateChartView() => ChartMargin = ChartMargin;

    private void UpdateYAxisName()
    {
        YAxis[0] = new()
        {
            Name = OpenedExchanges[SelectedOpenedIndex].Symbol.BaseCurrency,
            ShowSeparatorLines = true,
            NamePaint = new SolidColorPaint { Color = DefaultFontColor },
            LabelsPaint = new SolidColorPaint { Color = DefaultFontColor }
        };
    }
}
