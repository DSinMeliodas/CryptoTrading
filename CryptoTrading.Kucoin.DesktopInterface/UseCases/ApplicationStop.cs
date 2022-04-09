﻿using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

using System.ComponentModel;

namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public sealed class ApplicationStop : IContextBaseUseCase<CancelEventArgs>
{
    public void Execute(CancelEventArgs _)
    {
        DataHub.ShutDownAll();
    }
}