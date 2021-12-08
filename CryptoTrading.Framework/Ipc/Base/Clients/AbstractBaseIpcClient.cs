using CryptoTrading.Framework.Ipc.Interface.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.Participants;
using CryptoTrading.Framework.Parallelism;

using System;
using System.Threading;

namespace CryptoTrading.Framework.Ipc.Base.Clients
{
    internal abstract class AbstractBaseIpcClient<TListenerTarget> : IIpcClient<TListenerTarget>
        where TListenerTarget : IIpcListenerTarget
    {
        private static readonly TimeSpan Infinite = TimeSpan.FromMilliseconds(-1);

        public event IpcCommandReceived<TListenerTarget> OnCommandReceived;

        private bool m_Disposed;
        private TimeSpan m_UpdateTickRate;

        private readonly CancellationController m_ListeningController = new();
        private readonly CancellationController m_RunningController = new();
        private readonly Timer m_Timer;

        public bool IsListening => !ListeningToken.IsCancellationRequested;

        public bool IsRunning => !RunningToken.IsCancellationRequested;

        public TListenerTarget Target { get; init; }

        protected CancellationToken ListeningToken => m_ListeningController.Token;

        protected CancellationToken RunningToken => m_RunningController.Token;

        protected TimeSpan UpdateTickRate
        {
            get => m_UpdateTickRate;
            set
            {
                m_UpdateTickRate = value;
                _ = m_Timer.Change(TimeSpan.Zero, UpdateTickRate);
            }
        }

        protected AbstractBaseIpcClient(TimeSpan updateTickRate)
        {
            m_Timer = new Timer(UpdateTick);
            m_ListeningController.LinkTo(m_RunningController);
            m_UpdateTickRate = updateTickRate;
        }

        public void Dispose()
        {
            _ = Stop();
            m_Timer?.Dispose();
            m_RunningController.Dispose();
            m_ListeningController.Dispose();
            Dispose(!m_Disposed);
            m_Disposed = true;
        }

        public bool Start()
        {
            if (IsRunning || !OnStart() || !StartListeningInternal(true))
            {
                return false;
            }
            m_RunningController.ChangeSource(new CancellationTokenSource());
            return true;
        }

        public bool Stop()
        {
            if (!IsRunning || !OnStop() || !StopListening())
            {
                return false;
            }
            m_RunningController.Reset();
            return true;
        }

        public bool StartListening() => StartListeningInternal(false);

        public bool StopListening()
        {
            if (!IsListening || !OnListeningStop())
            {
                return false;
            }
            m_ListeningController.Reset();
            return m_Timer.Change(Infinite, UpdateTickRate);
        }

        public abstract IIpcCommandResult Send(IIpcCommand command);

        protected void ReceiveCommand(IIpcCommand command) => OnCommandReceived?.Invoke(this, command);

        protected virtual void Dispose(bool disposing)
        {
        }

        protected abstract bool OnListeningStart();

        protected abstract bool OnListeningStop();

        protected abstract bool OnStart();

        protected abstract bool OnStop();

        protected abstract void UpdateTick(object _);

        private bool StartListeningInternal(bool initialStart)
        {
            if (!initialStart && !IsRunning)
            {
                return false;
            }
            if (IsListening || !OnListeningStart())
            {
                return false;
            }
            m_ListeningController.ChangeSource(new CancellationTokenSource());
            return m_Timer.Change(TimeSpan.Zero, UpdateTickRate);
        }
    }
}