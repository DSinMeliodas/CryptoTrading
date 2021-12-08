using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

using System;

namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    /// <summary>
    /// Represents an sender that can send a command to another process.
    /// </summary>
    internal interface IIpcSender : IDisposable
    {
        /// <summary>
        /// Sends a command to another process and returns the result of the execution.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IIpcCommandResult Send(IIpcCommand command);
    }
}