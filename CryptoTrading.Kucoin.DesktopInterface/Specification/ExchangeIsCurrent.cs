using CryptoTrading.Kucoin.DesktopInterface.Backend.Exchange;
using CryptoTrading.Kucoin.DesktopInterface.Domain;

namespace CryptoTrading.Kucoin.DesktopInterface.Specification;

internal sealed class ExchangeIsCurrent : ISpecification<ExchangeIdentifier>
{
    private readonly IExchangeManager m_Manager;

    public ExchangeIsCurrent(IExchangeManager manager)
    {
        m_Manager = manager;
    }

    public bool IsMet(ExchangeIdentifier target)
    {
        var currentExchange = ExchangeManager.Instance.CurrentExchangeIdentifier;
        return target.Equals(currentExchange);
    }
}