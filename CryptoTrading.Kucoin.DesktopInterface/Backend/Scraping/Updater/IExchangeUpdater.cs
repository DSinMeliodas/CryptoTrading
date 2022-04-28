using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface IExchangeUpdater
{
    Task<Exchange> GetExchange(ExchangeIdentifier exchangeId);

    Task<object> MakeUpdateCall(IExchangeTarget target);
}