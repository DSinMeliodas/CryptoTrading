
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTrading.Kucoin.DesktopInterface.Backend.Extensions;

internal static class Collection
{
    public static void AddDifferenceToCollection<T>(
        this ICollection<T> collection,
        ICollection<T> totalSet,
        ICollection<T> excluded)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(totalSet);
        ArgumentNullException.ThrowIfNull(excluded);
        var difference = totalSet.Except(excluded);
        foreach (var element in difference)
        {
            collection.Add(element);
        }
    }
}