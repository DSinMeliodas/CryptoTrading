using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public record Candle(TimeSpan OpenTime, decimal Open, decimal Close, decimal High, decimal Low, decimal TradeVolume);