using System;
using System.Text.RegularExpressions;
using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain.Factories;

internal class ExchangeIdentifierFactory
{
    private static readonly Regex ExchangeIdentifierRegex = new(@"[a-zA-Z0-9]{3,5}\-[a-zA-Z0-9]{3,5}");
    private readonly string m_RawExchangeIdentifier;

    public ExchangeIdentifierFactory(string rawExchangeIdentifier)
    {
        m_RawExchangeIdentifier = rawExchangeIdentifier;
        if (!ExchangeIdentifierRegex.IsMatch(m_RawExchangeIdentifier))
        {
            throw new ArgumentException($"{nameof(rawExchangeIdentifier)} does not match regex {nameof(ExchangeIdentifierRegex)}");
        }
    }

    public ExchangeIdentifier Create()
    {
        var currencies = m_RawExchangeIdentifier.Split('-');
        return new ExchangeIdentifier(m_RawExchangeIdentifier, currencies[0], currencies[1]);
    }
}