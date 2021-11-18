using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

using System;

namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    internal delegate void IpcCommandReceived(IIpcCommand command);

    internal interface IIpcListener<TListenerTarget> : IDisposable where TListenerTarget : IIpcListenerTarget
    {
        event IpcCommandReceived OnCommandReceived;

        TListenerTarget Target { get; init; }

        bool StartListening();

        bool StopListening();
    }
}