using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal sealed class KucoinUpdateIntervalSettings : IUpdateIntervalSettings
{
    public event EventHandler<bool> OnAutoUpdatedChanged;
    public event EventHandler<TimeSpan> OnUpdateIntervalChanged;

    public static KucoinUpdateIntervalSettings Instance { get; } = new ();

    private KucoinUpdateIntervalSettings()
    {
    }

    public bool IsAutoUpdated { get; private set; } = true;

    public void ChangeAutoUpdated(bool newAutoUpdated)
    {
        IsAutoUpdated = newAutoUpdated;
        OnAutoUpdatedChanged?.Invoke(this, IsAutoUpdated);
    }

    public void ChangeUpdateInterval(TimeSpan newTickInterval) => OnUpdateIntervalChanged?.Invoke(this, newTickInterval);
}