namespace CryptoTrading.Kucoin.DesktopInterface.Adapters;

public interface IAdapter<TFrom, TTo>
{
    TTo ConvertFrom(TFrom value);
}