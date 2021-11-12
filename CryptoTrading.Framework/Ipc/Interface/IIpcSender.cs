using System;

namespace CryptoTrading.Framework.Ipc.Interface
{
    internal interface IIpcSender : IDisposable
    {
        IIpcCommandResult Send(IIpcCommand command);
    }
}