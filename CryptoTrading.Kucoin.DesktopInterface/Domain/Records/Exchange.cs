using Kucoin.Net.Enums;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

public record Exchange(ExchangeIdentifier Identifier, KlineInterval TimeInterval, Candle[] Course);