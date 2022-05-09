using CryptoTrading.Kucoin.DesktopInterface.Domain.Records;

namespace CryptoTrading.Kucoin.DesktopInterface.Specifications;

internal sealed class NeedsDefaultUpdateInterval : ISpecification<int>
{
    public bool IsMet(int target)
    {
        return target < 0 || target > UpdateInterval.AllIntervals.Length;
    }
}