﻿using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;

namespace CryptoTrading.Kucoin.DesktopInterface.Repositories.CallBacks;

public interface IExchangeUpdateCallBack
{
    void OnExchangeUpdate(Exchange currentExchange);
}