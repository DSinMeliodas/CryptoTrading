using System;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping;

public class TickUpdateSubscription : IEquatable<TickUpdateSubscription>
{
    private readonly Type m_Type;

    public Guid Id { get; init; }

    public ITickerTarget Target { get; init; }

    public TickUpdateSubscription(Guid id, ITickerTarget target, Type type)
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


    public override int GetHashCode() => Id.GetHashCode();

    public bool Equals(TickUpdateSubscription other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as TickUpdateSubscription);
    }
}