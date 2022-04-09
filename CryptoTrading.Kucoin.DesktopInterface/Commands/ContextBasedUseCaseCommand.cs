using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using GalaSoft.MvvmLight.CommandWpf;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal abstract class ContextBasedUseCaseCommand<TContext> : RelayCommand<TContext>
{
    protected ContextBasedUseCaseCommand(IContextBaseUseCase<TContext> useCase, bool keepTargetAlive = false)
        : base(useCase.Execute, keepTargetAlive)
    {
    }

    protected ContextBasedUseCaseCommand(IConditionalContextBasedUseCase<TContext> useCase, bool keepTargetAlive = false)
        : base(useCase.Execute, useCase.CanExecute, keepTargetAlive)
    {
    }
}