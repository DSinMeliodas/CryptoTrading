using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System.Threading.Tasks;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public interface IExchangeUpdater
{
    Task<Exchange> GetExchange(ExchangeIdentifier exchangeId);

    Task<object> MakeUpdateCall(DataTargetIdentifier target);
}