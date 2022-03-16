using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Kucoin.DesktopInterface.Domain;

internal class ExchangeSelector
{
    private const string InplaceInstanceId = nameof(InplaceInstance);

    private static readonly Dictionary<string, ExchangeSelector> m_Selectors = new();

    public ObservableCollection<string> Exchanges { get; private set; }

    private ExchangeSelector(ITickUpdater updater)
    {
    }

    internal static ExchangeSelector InplaceInstance()
    {
        if(m_Selectors.TryGetValue(InplaceInstanceId, out var selector)) {
            return selector;
        }
        selector = new ExchangeSelector(null);
        m_Selectors.Add(InplaceInstanceId, selector);
        return selector;
    }
}
