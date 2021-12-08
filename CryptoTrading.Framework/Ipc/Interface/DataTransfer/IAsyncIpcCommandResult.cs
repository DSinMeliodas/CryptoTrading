namespace CryptoTrading.Framework.Ipc.Interface.DataTransfer
{
    /// <summary>
    /// An asynchronous result.
    /// </summary>
    public interface IAsyncIpcCommandResult : IIpcCommandResult
    {
        /// <summary>
        /// Bool value indicating whether the result has been received.
        /// </summary>
        bool HasResult { get; }
    }
}