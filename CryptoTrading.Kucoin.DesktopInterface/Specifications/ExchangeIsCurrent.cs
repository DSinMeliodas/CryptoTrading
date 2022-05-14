using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Specifications;

internal sealed class ExchangeIsCurrent : ISpecification<ExchangeSymbol>
{
    private readonly IExchangeManager m_Manager;

    public ExchangeIsCurrent(IExchangeManager manager)
    {
        m_Manager = manager;
    }

    public bool IsMet(ExchangeSymbol target)
    {
        var currentExchange = m_Manager.CurrentExchangeSymbol;
        return target.Equals(currentExchange);
    }
}