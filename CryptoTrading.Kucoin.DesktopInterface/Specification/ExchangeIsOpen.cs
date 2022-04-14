using CryptoTrading.Kucoin.DesktopInterface.Backend.Exchange;
using CryptoTrading.Kucoin.DesktopInterface.Domain;

namespace CryptoTrading.Kucoin.DesktopInterface.Specification;

internal sealed class ExchangeIsOpen : ISpecification<ExchangeIdentifier>
{
    private readonly IExchangeManager m_Manager;

    public ExchangeIsOpen(IExchangeManager manager)
    {
        m_Manager = manager;
    }

    public bool IsMet(ExchangeIdentifier target)
    {
        return m_Manager.OpenExchangeIdentifiers.Contains(target);
    }
}