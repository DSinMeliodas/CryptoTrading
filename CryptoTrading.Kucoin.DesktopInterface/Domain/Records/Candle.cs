using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public record Candle(DateTime OpenTime, decimal Open, decimal Close, decimal High, decimal Low, decimal TradeVolumeInBase, decimal TradeVolumeInForeign);