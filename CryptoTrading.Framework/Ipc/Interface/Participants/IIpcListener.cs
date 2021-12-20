using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

namespace CryptoTrading.Framework.Ipc.Interface.Participants;

/// <summary>
/// Delegate that represents a method that will be called upon receiving a command.
/// </summary>
/// <typeparam name="TListenerTarget"></typeparam>
/// <param name="sender">The listener that received the command.</param>
/// <param name="command">The received command.</param>
internal delegate void IpcCommandReceived<TListenerTarget>(IIpcListener<TListenerTarget> sender, IIpcCommand command) where TListenerTarget : IIpcListenerTarget;

internal interface IIpcListener<TListenerTarget> : IDisposable where TListenerTarget : IIpcListenerTarget
{
    /// <summary>
    /// Event that will be invoked when the listener receives a command.
    /// </summary>
    event IpcCommandReceived<TListenerTarget> OnCommandReceived;

    /// <summary>
    /// Bool value indicating whether the listener is active or not.
    /// </summary>
    bool IsListening { get; }

    /// <summary>
    /// The target of the listener.
    /// </summary>
    TListenerTarget Target { get; init; }

    /// <summary>
    /// Starts the listener if it is not already listening.
    /// </summary>
    /// <returns>Bool value indicating whether the start was successful or not.</returns>
    bool StartListening();

    /// <summary>
    /// Stops the listener if it is listening.
    /// </summary>
    /// <returns>Bool value indicating whether the stop was successful or not.</returns>
    bool StopListening();
}