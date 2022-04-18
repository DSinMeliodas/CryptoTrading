namespace CryptoTrading.Kucoin.DesktopInterface.Specifications;

public interface ISpecification<TSpecTarget>
{
    bool IsMet(TSpecTarget target);
}