using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

using System;

namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    internal delegate void IpcCommandReceived<TListenerTarget>(IIpcListener<TListenerTarget> sender, IIpcCommand command) where TListenerTarget : IIpcListenerTarget;

    internal interface IIpcListener<TListenerTarget> : IDisposable where TListenerTarget : IIpcListenerTarget
    {
        event IpcCommandReceived<TListenerTarget> OnCommandReceived;

        TListenerTarget Target { get; init; }

        bool StartListening();

        bool StopListening();
    }
}