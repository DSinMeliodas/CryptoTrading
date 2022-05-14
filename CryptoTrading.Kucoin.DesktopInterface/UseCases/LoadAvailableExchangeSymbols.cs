using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public class LoadAvailableExchangeSymbols : IQueryUseCase<IExchangeSymbolsUpdateCallBack, IReadOnlyList<string>>
{
    private readonly IExchangeRepository m_ExchangeRepository;

    public LoadAvailableExchangeSymbols(IExchangeRepository exchangeRepository)
    {
        m_ExchangeRepository = exchangeRepository;
    }

    public IReadOnlyList<string> Execute(IExchangeSymbolsUpdateCallBack queryParameter)
    {
        return m_ExchangeRepository.GetAvailableExchanges(queryParameter).Result;
    }
}