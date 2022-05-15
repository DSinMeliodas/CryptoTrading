using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Util;

internal static class ThrowHelper
{
    [StackTraceHidden]
    public static void ThrowIfHigherThan(
        decimal value,
        decimal higherLimit,
        [CallerArgumentExpression("value")] string? valueExpression = null,
        [CallerArgumentExpression("higherLimit")]
        string limitExpression = null)
    {
        if (value > higherLimit)
        {
            throw new ArgumentOutOfRangeException(valueExpression, value,
                $"{valueExpression} cannot be higher than {limitExpression} which is {higherLimit}");
        }
    }

    [StackTraceHidden]
    public static void ThrowIfLowerThan(
        decimal value,
        decimal lowerLimit,
        [CallerArgumentExpression("value")] string? valueExpression = null,
        [CallerArgumentExpression("lowerLimit")] string limitExpression = null)
    {
        if (value < lowerLimit)
        {
            throw new ArgumentOutOfRangeException(valueExpression, value, $"{valueExpression} cannot be lower than {limitExpression} which is {lowerLimit}");
        }
    }

    [StackTraceHidden]
    public static void ThrowIfLowerThanOrEqual0(
        decimal value,
        [CallerArgumentExpression("value")] string? expression = null)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(expression, value, $"{expression} cannot be lower than or equal to 0");
        }
    }

    [StackTraceHidden]
    public static void ThrowIfHighOrLowNotSet<T>(
        decimal? high,
        decimal? low,
        T _,
        [CallerArgumentExpression("_")] string? expression = null)
    {
        if (high is null || low is null)
        {
            throw new InvalidOperationException($"cannot set {expression} until both low and high are set");
        }
    } 
}