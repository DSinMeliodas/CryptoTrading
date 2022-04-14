using Kucoin.Net.Enums;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain;

public record Exchange(ExchangeIdentifier Identifier, KlineInterval TimeInterval, Candle[] Course);