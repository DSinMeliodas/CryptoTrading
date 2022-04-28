using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Specifications;

internal sealed class ExchangeIsCurrent : ISpecification<ExchangeIdentifier>
{
    private readonly IExchangeManager m_Manager;

    public ExchangeIsCurrent(IExchangeManager manager)
    {
        m_Manager = manager;
    }

    public bool IsMet(ExchangeIdentifier target)
    {
        var currentExchange = m_Manager.CurrentExchangeIdentifier;
        return target.Equals(currentExchange);
    }
}