using CommunityToolkit.Mvvm.Input;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal abstract class RelayCommandBase<TContext> : IRelayCommand
{
    public event EventHandler? CanExecuteChanged;
    
    public bool CanExecute(object? parameter)
    {
        return parameter is TContext context && CanExecute(context);
    }

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }
        Execute((TContext)parameter);
    }

    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public abstract bool CanExecute(TContext? parameter);

    public abstract void Execute(TContext? parameter);
}