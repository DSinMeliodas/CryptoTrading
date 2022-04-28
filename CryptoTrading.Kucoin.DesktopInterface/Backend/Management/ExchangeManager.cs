using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;
using System.Collections.Generic;
using CryptoTrading.Kucoin.DesktopInterface.Backend.Util;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Management;

internal sealed class ExchangeManager : IExchangeManager
{
    public event ExchangeChanged OnOpenedExchangesChanged;

    private readonly List<ExchangeIdentifier> m_OpendExchangeIdentifiers = new();
    private readonly List<Exchange> m_OpendExchanges = new();

    public ExchangeIdentifier? CurrentExchangeIdentifier { get; private set; }

    public int CurrentIndex => m_OpendExchangeIdentifiers.IndexOf(CurrentExchangeIdentifier);

    public void Dispose()
    {
        OnOpenedExchangesChanged = null;
    }

    public bool IsOpen(ExchangeIdentifier target) => m_OpendExchangeIdentifiers.Contains(target);

    public void OpenExchange(Exchange exchange)
    {
        ArgumentNullException.ThrowIfNull(exchange);
        m_OpendExchangeIdentifiers.Add(exchange.Identifier);
        m_OpendExchanges.Add(exchange);
        CurrentExchangeIdentifier = exchange.Identifier;
        InvokeEvent(ChangeAction.Added | ChangeAction.Seleced);
    }

    public void CloseCurrentExchange()
    {
        var index = CurrentIndex;
        var removeIndex = index > 0 ? index - 1 : index + 1;
        CurrentExchangeIdentifier = removeIndex >= m_OpendExchangeIdentifiers.Count
                                    ? null 
                                    : m_OpendExchangeIdentifiers[removeIndex];
        RemoveAtInternal(index);
        InvokeEvent(ChangeAction.Seleced | ChangeAction.Removed);
    }

    public void CloseExchange(ExchangeIdentifier exchangeId)
    {
        ArgumentNullException.ThrowIfNull(exchangeId);
        if (exchangeId.Equals(CurrentExchangeIdentifier))
        {
            throw new ArgumentException($"cannot close current exchange using this method, consider using {nameof(CloseCurrentExchange)}");
        }
        var index = m_OpendExchangeIdentifiers.IndexOf(exchangeId);
        if (index < 0)
        {
            ThrowHelper.ThrowExchangeNotOpen(exchangeId);
        }
        RemoveAtInternal(index);
        InvokeEvent(ChangeAction.Removed);
    }

    public void SetOpenedAsCurrent(ExchangeIdentifier exchangeId)
    {
        ArgumentNullException.ThrowIfNull(exchangeId);
        if (!IsOpen(exchangeId))
        {
            ThrowHelper.ThrowExchangeNotOpen(exchangeId);
        }
        CurrentExchangeIdentifier = exchangeId;
        InvokeEvent(ChangeAction.Seleced);
    }

    public void UpdateExchange(Exchange exchange)
    {
        var index = m_OpendExchangeIdentifiers.IndexOf(exchange.Identifier);
        if (index < 0)
        {
            ThrowHelper.ThrowExchangeNotOpen(exchange.Identifier);
        }
        m_OpendExchanges[index] = exchange;
    }

    private void InvokeEvent(ChangeAction action)
    {
        var args = new ExchangeChangedArgs(CurrentIndex, m_OpendExchanges.ToArray(), action);
        OnOpenedExchangesChanged?.Invoke(CurrentIndex, args);
    }

    private void RemoveAtInternal(int index)
    {
        m_OpendExchanges.RemoveAt(index);
        m_OpendExchangeIdentifiers.RemoveAt(index);
    }
}