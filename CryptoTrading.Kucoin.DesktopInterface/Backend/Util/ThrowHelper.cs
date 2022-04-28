using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Util;

internal static class ThrowHelper
{
    [DoesNotReturn]
    [StackTraceHidden]
    public static void ThrowExchangeNotOpen(ExchangeIdentifier exchangeId)
    {
        throw new ArgumentException($"exchange {exchangeId} is not open");
    }
}