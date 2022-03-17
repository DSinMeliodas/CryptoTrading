﻿using CryptoTrading.Kucoin.DesktopInterface.Annotations;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

public interface ITickUpdater
{
    public event OnTickUpdate OnTickUpdate;

    TickUpdateSubscription Subscribe([NotNull]string target, [NotNull]Type targetType);
}