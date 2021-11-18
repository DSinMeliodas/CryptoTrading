using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

using System;

namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    internal interface IIpcSender : IDisposable
    {
        IIpcCommandResult Send(IIpcCommand command);
    }
}