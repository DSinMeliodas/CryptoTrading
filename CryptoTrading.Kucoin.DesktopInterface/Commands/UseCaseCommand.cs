using CommunityToolkit.Mvvm.Input;

using CryptoTrading.Kucoin.DesktopInterface.UseCases;

using System;

namespace CryptoTrading.Kucoin.DesktopInterface.Commands;

internal abstract class UseCaseCommand : IRelayCommand
{
    public event EventHandler? CanExecuteChanged;

    private readonly IUseCase m_UseCase;

    protected UseCaseCommand(IUseCase useCase)
    {
        m_UseCase = useCase;
    }

    public bool CanExecute(object? _) => true;

    public void Execute(object? _) => m_UseCase.Execute();

    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}