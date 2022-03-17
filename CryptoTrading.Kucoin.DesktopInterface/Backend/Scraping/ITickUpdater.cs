using CryptoTrading.Kucoin.DesktopInterface.Annotations;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

public interface ITickUpdater : IDisposable
{
    public event OnTickUpdate OnTickUpdate;

    TimeSpan UpdateInterval { get; set; }

    void Start();

    void Stop();

    TickUpdateSubscription Subscribe([NotNull]string target, [NotNull]Type targetType);
}