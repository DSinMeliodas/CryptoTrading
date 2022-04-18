using CryptoTrading.Kucoin.DesktopInterface.Domain.Factories;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;
using CryptoTrading.Kucoin.DesktopInterface.Repositories;
using CryptoTrading.Kucoin.DesktopInterface.UseCases.Requests;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

internal sealed class LoadExchangeUseCase : IQueryUseCase<IExchangeRequest, Exchange>
{
    private readonly IExchangeRepository m_ExchangeRepository;

    public LoadExchangeUseCase(IExchangeRepository exchangeRepository)
    {
        m_ExchangeRepository = exchangeRepository;
    }

    public Exchange Execute(IExchangeRequest request)
    {
        var idFactory = new ExchangeIdentifierFactory(request.ExchangeId);
        return m_ExchangeRepository.GetExchange(idFactory.Create(), request.UpdateCallBack);
    }
}