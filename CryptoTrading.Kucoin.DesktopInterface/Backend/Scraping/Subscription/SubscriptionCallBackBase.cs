using CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Updater;

using System;
using System.Windows;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Scraping.Subscription;

internal abstract class SubscriptionCallBackBase : ISubscriptionCallBack
{
    public abstract void OnTickUpdate(TickUpdateEventArgs args);

    protected bool CheckArgs<TResult>(TickUpdateEventArgs args, out TResult result)
    {
        ArgumentNullException.ThrowIfNull(args);
        result = default;
        if (args.IsError)
        {
            ErrorAction(args);
            return false;
        }

        if (args.Result is not TResult casted)
        {
            WrongResultTypeAction(args);
            return false;
        }
        result = casted;
        return true;
    }

    protected virtual void ErrorAction(TickUpdateEventArgs args)
    {
#if DEBUG
        MessageBox.Show("Failed retrieving result");
#endif
    }

    protected virtual void WrongResultTypeAction(TickUpdateEventArgs args)
    {
#if DEBUG
        MessageBox.Show(args.Error?.ToString());
#endif
    }
}