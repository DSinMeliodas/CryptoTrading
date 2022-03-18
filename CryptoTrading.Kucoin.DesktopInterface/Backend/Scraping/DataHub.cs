using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

internal class DataHub : ITickUpdater
{
    public const string InPlaceInstanceId = "Inplace";

    private static DataHub Instance => s_HubInstance ??= new DataHub();
    private static DataHub s_HubInstance;

    public event Action<Task<CallResult<object>>> OnAsyncCallError
    {
        add => m_CurrentUpdater.OnAsyncCallError += value;
        remove => m_CurrentUpdater.OnAsyncCallError -= value;
    }

    public event Action<CallResult<object>> OnCallError
    {
        add => m_CurrentUpdater.OnCallError += value;
        remove => m_CurrentUpdater.OnCallError -= value;
    }

    public event OnTickUpdate OnTickUpdate
    {
        add => m_CurrentUpdater.OnTickUpdate += value;
        remove => m_CurrentUpdater.OnTickUpdate -= value;
    }

    private readonly Dictionary<string, ITickUpdater> m_Updaters = new();

    private string m_CurrentUpdaterIdentifier;
    private ITickUpdater m_CurrentUpdater;

    public TimeSpan UpdateInterval
    {
        get => m_CurrentUpdater.UpdateInterval;
        set => m_CurrentUpdater.UpdateInterval = value;
    }

    private DataHub()
    {
    }

    public void Dispose()
    {
        m_CurrentUpdater.Dispose();
        foreach (var updaters in m_Updaters.Values)
        {
            updaters.Dispose();
        }
    }

    public static ITickUpdater UseInPlaceUpdater(bool deleteCurrentUpdater)
    {
        if (Instance.m_CurrentUpdater is not null)
        {
            _ = Instance.m_CurrentUpdater.Stop();
            if (deleteCurrentUpdater)
            {
                _ = Instance.m_Updaters.Remove(Instance.m_CurrentUpdaterIdentifier);
                Instance.m_CurrentUpdater.Dispose();
            }
        }
        Instance.m_CurrentUpdaterIdentifier = InPlaceInstanceId;
        if (Instance.m_Updaters.TryGetValue(InPlaceInstanceId, out var inplaceUpdater))
        {
            Instance.m_CurrentUpdater = inplaceUpdater;
            return Instance;
        }

        inplaceUpdater = new KucoinTickUpdater();
        Instance.m_CurrentUpdater = inplaceUpdater;
        Instance.m_Updaters.Add(Instance.m_CurrentUpdaterIdentifier, Instance.m_CurrentUpdater);
        return Instance;
    }

    public bool Start()
    {
        return m_CurrentUpdater.Start();
    }

    public bool Stop()
    {
        return m_CurrentUpdater.Stop();
    }

    public TickUpdateSubscription Subscribe(ITickerTarget target)
    {
        return m_CurrentUpdater.Subscribe(target);
    }

    public void Unsubscribe(TickUpdateSubscription subscription)
    {
        m_CurrentUpdater.Unsubscribe(subscription);
    }
}