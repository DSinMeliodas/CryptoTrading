namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public interface IConditionalContextBasedUseCase<TContext> : IContextBaseUseCase<TContext>
{
    bool CanExecute(TContext context);
}