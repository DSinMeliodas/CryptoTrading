using CryptoTrading.Kucoin.DesktopInterface.Backend.Management;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Factories;

using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal class RemoveExchanges : IContextBasedUseCase<IReadOnlyList<string>>
{
    private readonly IExchangeManager m_ExchangeManager;

    public RemoveExchanges(IExchangeManager exchangeManager)
    {
        m_ExchangeManager = exchangeManager;
    }

    public void Execute(IReadOnlyList<string> context)
    {
        foreach (var removedExchange in context)
        {
            var exchangeFactory = new ExchangeIdentifierFactory(removedExchange);
            var exchangeId = exchangeFactory.Create();
            if (exchangeId.Equals(m_ExchangeManager.CurrentExchangeIdentifier))
            {
                m_ExchangeManager.CloseCurrentExchange();
                continue;
            }
            m_ExchangeManager.CloseExchange(exchangeId);
        }
    }
}