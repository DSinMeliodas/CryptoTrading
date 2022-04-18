namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public interface IQueryUseCase<TParam, TResult>
{
    TResult Execute(TParam queryParameter);
}