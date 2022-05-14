using System;
using System.Text.RegularExpressions;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Factories;

internal class ExchangeSymbolFactory
{
    private static readonly Regex ExchangeSymbolRegex = new(@"[a-zA-Z0-9]{3,5}\-[a-zA-Z0-9]{3,5}");
    private readonly string m_RawExchangeSymbol;

    public ExchangeSymbolFactory(string rawExchangeSymbol)
    {
        m_RawExchangeSymbol = rawExchangeSymbol;
        if (!ExchangeSymbolRegex.IsMatch(m_RawExchangeSymbol))
        {
            throw new ArgumentException($"{nameof(rawExchangeSymbol)} does not match regex {nameof(ExchangeSymbolRegex)}");
        }
    }

    public ExchangeSymbol Create()
    {
        var currencies = m_RawExchangeSymbol.Split('-');
        return new ExchangeSymbol(m_RawExchangeSymbol, currencies[0], currencies[1]);
    }
}