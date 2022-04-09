namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

public interface ITickUpdaterTarget : ITickerTarget
{
    string UpdaterId { get; }
}