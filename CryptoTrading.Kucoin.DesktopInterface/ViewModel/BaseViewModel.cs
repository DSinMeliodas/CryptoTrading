﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryptoTrading.Kucoin.DesktopInterface.ViewModel;

internal abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected abstract void Dispose(bool disposing);
}