using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

internal sealed class KucoinUpdateIntervalSettings : IUpdateIntervalSettings
{
    public event EventHandler<TimeSpan> OnUpdateIntervalChanged;

    public static KucoinUpdateIntervalSettings Instance { get; } = new ();

    private KucoinUpdateIntervalSettings()
    {
    }

    public void ChangeUpdateInterval(TimeSpan newTickInterval) => OnUpdateIntervalChanged?.Invoke(this, newTickInterval);
}