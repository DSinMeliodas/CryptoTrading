using CryptoTrading.Kucoin.DesktopInterface.Backend.Exchange;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Factories;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Specifications;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal sealed class SelectExchange 
{
    private readonly IExchangeManager m_Manager;
    private readonly ExchangeIdentifier m_SelectedExchangeIdentifier;
    private readonly ISpecification<ExchangeIdentifier> m_ExchangeIsCurrent;
    private readonly ISpecification<ExchangeIdentifier> m_ExchangeIsOpen;

    public SelectExchange(IExchangeManager manager, string selectedExchangeIdentifier)
    {
        m_Manager = manager;
        var identifierFactory = new ExchangeIdentifierFactory(selectedExchangeIdentifier);
        m_SelectedExchangeIdentifier = identifierFactory.Create();
        m_ExchangeIsCurrent = new ExchangeIsCurrent(m_Manager);
        m_ExchangeIsOpen = new ExchangeIsOpen(m_Manager);
    }

    public void Execute()
    {
        if (m_ExchangeIsCurrent.IsMet(m_SelectedExchangeIdentifier))
        {
            return;
        }
        if (m_ExchangeIsOpen.IsMet(m_SelectedExchangeIdentifier))
        {
            m_Manager.SetOpenedAsCurrent(m_SelectedExchangeIdentifier);
            return;
        }

        m_Manager.OpenExchange(m_SelectedExchangeIdentifier);
    }
}