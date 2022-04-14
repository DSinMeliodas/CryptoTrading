namespace CryptoTrading.Kucoin.DesktopInterface;

public interface ISpecification<TSpecTarget>
{
    bool IsMet(TSpecTarget target);
}