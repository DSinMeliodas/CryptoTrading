namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public interface IContextBaseUseCase<TContext>
{
    void Execute(TContext context);
}