using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler PropertyChanged;

    private readonly Dispatcher m_Dispatcher;

    protected BaseViewModel()
    {
        m_Dispatcher = Dispatcher.CurrentDispatcher;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void InvokeOnDispatcher(Action action) => m_Dispatcher.Invoke(action);

    protected void InvokeOnDispatcher<TParameter>(Action<TParameter> action, TParameter parameter) =>
        m_Dispatcher.Invoke(action, args: parameter);

    protected abstract void Dispose(bool disposing);
}