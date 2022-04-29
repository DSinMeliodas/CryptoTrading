using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Util;

internal static class ThrowHelper
{
    [DoesNotReturn]
    [StackTraceHidden]
    public static void ThrowExchangeNotOpen(ExchangeIdentifier exchangeId)
    {
        throw new ArgumentException($"exchange {exchangeId} is not open");
    }
    
    [StackTraceHidden]
    public static void ThrowIfCallError(CallResult call)
    {
        if (!call)
        {
            throw new Exception(call.Error!.Message);
        }
    }

    public static void ThrowIfUndefined(DataTargetIdentifier identifier, [CallerArgumentExpression("identifier")] string? expression = null)
    {
        if (identifier == DataTargetIdentifier.Undefined)
        {
            throw new ArgumentException($"{expression} is undefined", expression);
        }
    }
}