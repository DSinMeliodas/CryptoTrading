namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public interface IContextBasedUseCase<TContext>
{
    void Execute(TContext context);
}