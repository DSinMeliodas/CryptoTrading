using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using GalaSoft.MvvmLight.CommandWpf;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

public class UseCaseCommand : RelayCommand
{
    public UseCaseCommand(IUseCase useCase, bool keepTargetAlive = false)
        : base(useCase.Execute, keepTargetAlive)
    {
    }

    public UseCaseCommand(IConditionalUseCase conditionalUseCase, bool keepTargetAlive = false)
        : base(conditionalUseCase.Execute, conditionalUseCase.CanExecute, keepTargetAlive)
    {
    }
}