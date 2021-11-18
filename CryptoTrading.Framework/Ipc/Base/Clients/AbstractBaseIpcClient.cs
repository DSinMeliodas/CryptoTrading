using CryptoTrading.Framework.Ipc.Interface.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.Participants;

using System;
using System.Threading;

namespace CryptoTrading.Framework.Ipc.Base.Clients
{
    internal abstract class AbstractBaseIpcClient<TListenerTarget> : IIpcClient<TListenerTarget>
        where TListenerTarget : IIpcListenerTarget
    {
        public event IpcCommandReceived OnCommandReceived;

        private bool m_Disposed;
        private TimeSpan m_UpdateTickRate;

        private readonly Timer m_Timer;

        public TListenerTarget Target { get; init; }

        protected TimeSpan UpdateTickRate
        {
            get=>m_UpdateTickRate;
            set
            {
                m_UpdateTickRate = value;
                _ = m_Timer.Change(TimeSpan.Zero, UpdateTickRate);
            }
        }

        protected AbstractBaseIpcClient(TimeSpan updateTickRate)
        {
            m_Timer = new Timer(UpdateTick);
            UpdateTickRate = updateTickRate;
        }

        public void Dispose()
        {
            Stop();
            m_Timer?.Dispose();
            Dispose(!m_Disposed);
            m_Disposed = true;
        }

        public virtual void Start()
        {
            _ = StartListening();
        }

        public virtual void Stop()
        {
            _ = StopListening();
        }

        public abstract IIpcCommandResult Send(IIpcCommand command);

        protected abstract void UpdateTick(object _);

        public abstract bool StartListening();

        public abstract bool StopListening();

        protected virtual void Dispose(bool disposing)
        {
        }

        protected void ReceiveCommand(IIpcCommand command) => OnCommandReceived?.Invoke(command);
    }
}