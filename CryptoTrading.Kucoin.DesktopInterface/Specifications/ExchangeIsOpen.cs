using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Specifications;

internal sealed class ExchangeIsOpen : ISpecification<ExchangeIdentifier>
{
    private readonly IExchangeManager m_Manager;

    public ExchangeIsOpen(IExchangeManager manager)
    {
        m_Manager = manager;
    }

    public bool IsMet(ExchangeIdentifier target) => m_Manager.IsOpen(target);
}