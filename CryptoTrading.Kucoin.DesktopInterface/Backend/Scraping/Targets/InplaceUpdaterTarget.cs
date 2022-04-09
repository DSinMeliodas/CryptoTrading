using CryptoExchange.Net.Objects;

using Kucoin.Net.Clients;

using System;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public class InplaceUpdaterTarget : ITickUpdaterTarget
{
    private readonly ITickerTarget m_ActualTarget;

    public Type ResultType => m_ActualTarget.ResultType;

    public string UpdaterId => DataHub.InPlaceInstanceId;

    public InplaceUpdaterTarget(ITickerTarget actualTarget)
    {
        m_ActualTarget = actualTarget;
    }

    public Task<CallResult<object>> UpdateOn(KucoinClient client) => m_ActualTarget.UpdateOn(client);
}