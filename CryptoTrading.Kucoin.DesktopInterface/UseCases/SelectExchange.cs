using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Specifications;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal sealed class SelectExchange : IContextBasedUseCase<Exchange>
{
    private readonly IExchangeManager m_Manager;
    private readonly ISpecification<ExchangeSymbol> m_ExchangeIsCurrent;
    private readonly ISpecification<ExchangeSymbol> m_ExchangeIsOpen;

    public SelectExchange(IExchangeManager manager)
    {
        m_Manager = manager;
        m_ExchangeIsCurrent = new ExchangeIsCurrent(m_Manager);
        m_ExchangeIsOpen = new ExchangeIsOpen(m_Manager);
    }

    public void Execute(Exchange exchange)
    {
        if (m_ExchangeIsCurrent.IsMet(exchange.Symbol))
        {
            return;
        }
        if (m_ExchangeIsOpen.IsMet(exchange.Symbol))
        {
            m_Manager.SetOpenedAsCurrent(exchange.Symbol);
            return;
        }

        m_Manager.OpenExchange(exchange);
    }
}