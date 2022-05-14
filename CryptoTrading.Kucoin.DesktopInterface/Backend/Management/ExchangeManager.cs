using CryptoTrading.Kucoin.DesktopInterface.Backend.Util;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Entities;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

using System;
using System.Collections.Generic;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Management;

internal sealed class ExchangeManager : IExchangeManager
{
    public event ExchangeChanged OnOpenedExchangesChanged;

    private readonly List<ExchangeSymbol> m_OpendExchangeSymbols = new();
    private readonly List<Exchange> m_OpendExchanges = new();

    public ExchangeSymbol? CurrentExchangeSymbol { get; private set; }

    public int CurrentIndex => m_OpendExchangeSymbols.IndexOf(CurrentExchangeSymbol);

    public void Dispose()
    {
        OnOpenedExchangesChanged = null;
    }

    public bool IsOpen(ExchangeSymbol target) => m_OpendExchangeSymbols.Contains(target);

    public void OpenExchange(Exchange exchange)
    {
        ArgumentNullException.ThrowIfNull(exchange);
        if (IsOpen(exchange.Symbol))
        {
            throw new ArgumentException($"{exchange.Symbol} is already open");
        }
        m_OpendExchangeSymbols.Add(exchange.Symbol);
        m_OpendExchanges.Add(exchange);
        CurrentExchangeSymbol = exchange.Symbol;
        InvokeEvent(ChangeAction.Added | ChangeAction.Seleced);
    }

    public void CloseCurrentExchange()
    {
        var index = CurrentIndex;
        var removeIndex = index > 0 ? index - 1 : index + 1;
        CurrentExchangeSymbol = removeIndex >= m_OpendExchangeSymbols.Count
                                    ? null 
                                    : m_OpendExchangeSymbols[removeIndex];
        RemoveAtInternal(index);
        InvokeEvent(ChangeAction.Seleced | ChangeAction.Removed);
    }

    public void CloseExchange(ExchangeSymbol exchangeId)
    {
        ArgumentNullException.ThrowIfNull(exchangeId);
        if (exchangeId.Equals(CurrentExchangeSymbol))
        {
            throw new ArgumentException($"cannot close current exchange using this method, consider using {nameof(CloseCurrentExchange)}");
        }
        var index = m_OpendExchangeSymbols.IndexOf(exchangeId);
        if (index < 0)
        {
            ThrowHelper.ThrowExchangeNotOpen(exchangeId);
        }
        RemoveAtInternal(index);
        InvokeEvent(ChangeAction.Removed);
    }

    public void SetOpenedAsCurrent(ExchangeSymbol exchangeId)
    {
        ArgumentNullException.ThrowIfNull(exchangeId);
        if (!IsOpen(exchangeId))
        {
            ThrowHelper.ThrowExchangeNotOpen(exchangeId);
        }
        CurrentExchangeSymbol = exchangeId;
        InvokeEvent(ChangeAction.Seleced);
    }

    public void UpdateExchange(Exchange exchange)
    {
        var index = m_OpendExchangeSymbols.IndexOf(exchange.Symbol);
        if (index < 0)
        {
            ThrowHelper.ThrowExchangeNotOpen(exchange.Symbol);
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
        m_OpendExchangeSymbols.RemoveAt(index);
    }
}