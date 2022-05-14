using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Factories;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal sealed class LoadExchange : IQueryUseCase<IExchangeRequest, Exchange>
{
    private readonly IExchangeRepository m_ExchangeRepository;

    public LoadExchange(IExchangeRepository exchangeRepository)
    {
        m_ExchangeRepository = exchangeRepository;
    }

    public Exchange Execute(IExchangeRequest request)
    {
        var idFactory = new ExchangeSymbolFactory(request.ExchangeSymbol);
        return m_ExchangeRepository.GetExchange(idFactory.Create(), request.UpdateCallBack).Result;
    }
}