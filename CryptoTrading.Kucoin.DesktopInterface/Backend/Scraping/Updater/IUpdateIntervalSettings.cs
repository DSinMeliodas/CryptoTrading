using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface IUpdateIntervalSettings
{
    event EventHandler<TimeSpan> OnUpdateIntervalChanged;

    void ChangeUpdateInterval(TimeSpan newTickInterval);
}