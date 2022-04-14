﻿
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;

using System;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

public sealed class TickUpdateSubscription : IEquatable<TickUpdateSubscription>
{
    private readonly Type m_Type;

    public ISubscriptionCallBack CallBack { get; }
    public Guid Id { get; }
    public ITickerTarget Target { get; }

    public TickUpdateSubscription(Guid id, ITickerTarget target, ISubscriptionCallBack callBack, Type type)
    {
        CallBack = callBack;
        Id = id;
        Target = target;
        m_Type = type;
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

    public async void NotifyTickUpdate(Task<CallResult<object>> updateResult)
    {
        var args = await TickUpdateEventArgs.CreateFromAsync(this, updateResult);
        CallBack.OnTickUpdate(args);
    }

    public bool TryCastSubscribed<T>(object subscribedValue, out T castedValue)
    {
        if (typeof(T).IsAssignableFrom(m_Type))
        {
            castedValue = (T)subscribedValue;
            return true;
        }

        castedValue = default;
        return false;
    }
}