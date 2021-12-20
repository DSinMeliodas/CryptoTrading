namespace CryptoTrading.Framework.Ipc.Interface.Participants;

/// <summary>
/// Represents a client that can communicate between processes. 
/// </summary>
/// <typeparam name="TListenerTarget"></typeparam>
internal interface IIpcClient<TListenerTarget> : IIpcListener<TListenerTarget>, IIpcSender
    where TListenerTarget: IIpcListenerTarget
{
    /// <summary>
    /// Bool value whether the client is running or not. <br/>
    /// <b>Warning</b>: a running client does not have to be listening.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Starts the client if it is not already running.
    /// </summary>
    /// <returns>Bool value indicating whether the start was successful or not.</returns>
    bool Start();

    /// <summary>
    /// Stops the client if it is running.
    /// </summary>
    /// <returns>Bool value indicating whether the stop was successful or not.</returns>
    bool Stop();
}