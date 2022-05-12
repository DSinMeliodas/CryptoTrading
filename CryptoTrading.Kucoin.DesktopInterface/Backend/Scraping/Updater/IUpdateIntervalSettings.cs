using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface IUpdateIntervalSettings
{
    event EventHandler<TimeSpan> OnUpdateIntervalChanged;
    event EventHandler<bool> OnAutoUpdatedChanged;

    bool IsAutoUpdated { get; }

    void ChangeUpdateInterval(TimeSpan newTickInterval);
    void ChangeAutoUpdated(bool newAutoUpdated);
}