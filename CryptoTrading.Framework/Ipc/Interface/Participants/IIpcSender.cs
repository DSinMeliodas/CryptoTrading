using CryptoTrading.Framework.Ipc.Interface.Data;

using System;

namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    internal interface IIpcSender : IDisposable
    {
        IIpcCommandResult Send(IIpcCommand command);
    }
}