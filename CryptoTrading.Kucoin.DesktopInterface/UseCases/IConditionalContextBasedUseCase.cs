namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public interface IConditionalContextBasedUseCase<TContext> : IContextBasedUseCase<TContext>
{
    bool CanExecute(TContext context);
}