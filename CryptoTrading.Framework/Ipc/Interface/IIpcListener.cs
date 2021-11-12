using System;

namespace CryptoTrading.Framework.Ipc.Interface
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