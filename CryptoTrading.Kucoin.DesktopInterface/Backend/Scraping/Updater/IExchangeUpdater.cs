using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface IExchangeUpdater
{
    Task<Exchange> GetExchange(ExchangeIdentifier exchangeId);
}