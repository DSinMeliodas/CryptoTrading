namespace CryptoTrading.Framework.Util;

public static class Util
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull(object value, [CallerArgumentExpression("value")] string valueExpression = null)
    {
        if(value is null)
        {
            throw new ArgumentNullException(valueExpression);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotMet(bool condition, [CallerArgumentExpression("condition")] string conditionExpression = null)
    {
        if (!condition)
        {
            throw new ArgumentException($"{conditionExpression} was not met");
        }
    }
}

