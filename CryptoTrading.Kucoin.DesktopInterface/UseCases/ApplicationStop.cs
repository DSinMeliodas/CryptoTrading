
using CryptoTrading.Kucoin.DesktopInterface.Repositories;

using System.ComponentModel;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public sealed class ApplicationStop : IContextBasedUseCase<CancelEventArgs>
{
    public void Execute(CancelEventArgs _)
    {
        KucoinExchangeRepository.Close();
    }
}