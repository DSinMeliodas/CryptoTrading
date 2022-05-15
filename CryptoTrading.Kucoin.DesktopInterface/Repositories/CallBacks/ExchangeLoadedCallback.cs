
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

internal sealed class ExchangeLoadedCallback : IExchangeUpdateCallBack
{
    public event Action<Exchange> OnLoaded; 

    public void OnExchangeUpdate(Exchange currentExchange)
    {
        OnLoaded?.Invoke(currentExchange);
    }
}