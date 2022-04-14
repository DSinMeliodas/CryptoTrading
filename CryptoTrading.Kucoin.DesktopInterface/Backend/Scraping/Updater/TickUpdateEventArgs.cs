using CryptoExchange.Net.Objects;

using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

using System;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public delegate void OnTickUpdate(TickUpdateEventArgs args);

public class TickUpdateEventArgs : EventArgs
{
    public Task<CallResult<object>> AsyncCallResult { get; }
    public CallResult<object> CallResult { get; }
    public Exception? Error { get; }
    public bool IsError { get; }
    public object? Result { get; }

    private readonly TickUpdateSubscription m_Subscription;

    private TickUpdateEventArgs(
        TickUpdateSubscription subscription,
        Task<CallResult<object>> asyncCallResult,
        Exception error) : this(subscription, asyncCallResult, null, error)
    {
    }

    private TickUpdateEventArgs(
        TickUpdateSubscription subscription,
        Task<CallResult<object>> asyncCallResult,
        CallResult<object> callResult,
        Exception error)
    {
        AsyncCallResult = asyncCallResult;
        CallResult = callResult;
        Error = error;
        IsError = true;
        m_Subscription = subscription;
    }

    private TickUpdateEventArgs(
        TickUpdateSubscription subscription,
        Task<CallResult<object>> asyncCallResult,
        CallResult<object> callResult,
        object result)
    {
        AsyncCallResult = asyncCallResult;
        CallResult = callResult;
        IsError = false;
        Result = result;
        m_Subscription = subscription;
    }

    public bool TryGetCastedResult<T>(out T subscriptionValue)
    {
        subscriptionValue = default;
        return !IsError && m_Subscription.TryCastSubscribed(Result, out subscriptionValue);
    }

    public static async Task<TickUpdateEventArgs> CreateFromAsync(TickUpdateSubscription subscription, Task<CallResult<object>> asyncCallResult)
    {
        try
        {
            var call = await asyncCallResult;
            if (!call)
            {
                return new TickUpdateEventArgs(subscription, asyncCallResult, call, new Exception(call.Error!.ToString()));
            }
            return new TickUpdateEventArgs(subscription, asyncCallResult, call, call.Data);
        }
        catch (Exception exception)
        {
            return new TickUpdateEventArgs(subscription, asyncCallResult, exception);
        }
    }
}