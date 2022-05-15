using CryptoTrading.Kucoin.DesktopInterface.UseCases;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal abstract class ContextBasedUseCaseCommand<TContext> : RelayCommandBase<TContext>
{
    private readonly IContextBasedUseCase<TContext> m_UseCase;

    protected ContextBasedUseCaseCommand(IContextBasedUseCase<TContext> useCase)
    {
        m_UseCase = useCase;
    }

    public override bool CanExecute(TContext? parameter) => parameter is not null;

    public override void Execute(TContext? parameter) => m_UseCase.Execute(parameter);
}