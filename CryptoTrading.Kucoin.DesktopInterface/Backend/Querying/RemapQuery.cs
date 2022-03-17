using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Querying;

internal class RemapQuery<TGroupKey, TInnerGroupValue, TOuterGroupValue>
{
    private IDictionary<IGrouping<TGroupKey, TInnerGroupValue>, TOuterGroupValue> m_Groups;

    private RemapQuery(IDictionary<IGrouping<TGroupKey, TInnerGroupValue>, TOuterGroupValue> groups)
    {
        m_Groups = groups;
    }

    public Dictionary<TInnerGroupValue, TOuterGroupValue> RemapInnerValueToOuterValue()
    {
        var remappedValues = new Dictionary<TInnerGroupValue, TOuterGroupValue>();
        foreach (var group in m_Groups)
        {
            var keyGroup = group.Key;
            foreach (var innerGroupValue in keyGroup)
            {
                remappedValues.Add(innerGroupValue, group.Value);
            }
        }
        return remappedValues;
    }

    public static RemapQuery<TGroupKey, TInnerGroupValue, TOuterGroupValue>
        ForAll(IDictionary<IGrouping<TGroupKey, TInnerGroupValue>, TOuterGroupValue> valuesMappedByGroup)
    {
        return new RemapQuery<TGroupKey, TInnerGroupValue, TOuterGroupValue>(valuesMappedByGroup);
    }
}