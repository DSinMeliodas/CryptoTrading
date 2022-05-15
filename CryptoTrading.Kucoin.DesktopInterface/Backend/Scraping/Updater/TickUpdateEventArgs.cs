
using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

using System;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

public delegate void OnTickUpdate(TickUpdateEventArgs args);

public sealed class TickUpdateEventArgs : EventArgs
{
    public Task<object> AsyncResult { get; }
    public Exception? Error { get; }
    public bool IsError { get; }
    public object? Result { get; }

    private readonly TickUpdateSubscription m_Subscription;
    
    private TickUpdateEventArgs(
        TickUpdateSubscription subscription,
        Task<object> asyncResult,
        Exception error)
    {
        AsyncResult = asyncResult;
        Error = error;
        IsError = true;
        m_Subscription = subscription;
    }

    private TickUpdateEventArgs(
        TickUpdateSubscription subscription,
        Task<object> asyncResult,
        object result)
    {
        AsyncResult = asyncResult;
        IsError = false;
        Result = result;
        m_Subscription = subscription;
    }

    public static async Task<TickUpdateEventArgs> CreateFromAsync(TickUpdateSubscription subscription, Task<object> asyncResult)
    {
        try
        {
            var result = await asyncResult;
            return new TickUpdateEventArgs(subscription, asyncResult, result);
        }
        catch (Exception exception)
        {
            return new TickUpdateEventArgs(subscription, asyncResult, exception);
        }
    }
}