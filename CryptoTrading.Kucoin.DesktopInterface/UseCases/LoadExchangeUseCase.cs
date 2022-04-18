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

    public Exchange Execute(IExchangeRequest request) => m_ExchangeRepository.GetExchange(request.ExchangeId, request.UpdateCallBack);
}