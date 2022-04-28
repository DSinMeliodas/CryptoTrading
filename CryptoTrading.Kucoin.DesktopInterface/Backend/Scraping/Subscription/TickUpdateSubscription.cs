
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Targets;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using System;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

public sealed class TickUpdateSubscription : IEquatable<TickUpdateSubscription>
{
    public ISubscriptionCallBack CallBack { get; }
    public Guid Id { get; }
    public IExchangeTarget Target { get; }

    public TickUpdateSubscription(Guid id, IExchangeTarget target, ISubscriptionCallBack callBack)
    {
        CallBack = callBack;
        Id = id;
        Target = target;
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

    public async Task NotifyTickUpdate(Task<object> updateResult)
    {
        var args = await TickUpdateEventArgs.CreateFromAsync(this, updateResult);
        CallBack.OnTickUpdate(args);
    }
}