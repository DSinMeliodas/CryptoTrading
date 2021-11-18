namespace CryptoTrading.Framework.Ipc.Interface.DataTransfer
{
    public interface IAsyncIpcCommandResult : IIpcCommandResult
    {
        bool HasResult { get; }
    }
}