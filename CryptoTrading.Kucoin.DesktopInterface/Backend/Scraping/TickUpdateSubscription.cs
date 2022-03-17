using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

public class TickUpdateSubscription
{
    private readonly Type m_Type;

    public long Id { get; init; }

    public string Target { get; init; }

    public TickUpdateSubscription(long id, string target, Type type)
    {
        Id = id;
        Target = target;
        m_Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    internal bool TryCastSubscribed<T>(object subscribedValue, out T castedValue)
    {
        if(typeof(T).IsAssignableFrom(m_Type))
        {
            castedValue = (T)subscribedValue;
            return true;
        }

        castedValue = default;
        return false;
    }
}