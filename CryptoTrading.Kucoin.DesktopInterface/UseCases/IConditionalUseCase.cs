namespace CryptoTrading.Kucoin.DesktopInterface.UseCases;

public interface IConditionalUseCase : IUseCase
{
    bool CanExecute();
}