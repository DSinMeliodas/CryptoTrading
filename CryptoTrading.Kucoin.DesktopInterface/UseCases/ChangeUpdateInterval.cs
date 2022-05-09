using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Specifications;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal sealed class ChangeUpdateInterval : IContextBasedUseCase<int>
{
    private readonly IUpdateIntervalSettings m_UpdateIntervalSettings;
    private readonly ISpecification<int> m_NeedsDefaultUpdateInterval = new NeedsDefaultUpdateInterval();

    public ChangeUpdateInterval(IUpdateIntervalSettings updateIntervalSettings)
    {
        m_UpdateIntervalSettings = updateIntervalSettings;
    }

    public void Execute(int context)
    {
        var interval = m_NeedsDefaultUpdateInterval.IsMet(context)
            ? UpdateInterval.Default
            : UpdateInterval.AllIntervals[context];
        m_UpdateIntervalSettings.ChangeUpdateInterval(interval.Interval);
    }
}