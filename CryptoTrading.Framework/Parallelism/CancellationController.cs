using CryptoTrading.Framework.EventArgs;
using CryptoTrading.Framework.Util.Cancellation;

using System;
using System.Collections.Generic;
using System.Threading;

namespace CryptoTrading.Framework.Parallelism
{
    internal sealed class CancellationController : IDisposable
    {
        public event Action OnCancellationRequested;
        public event EventHandler<ObjectDisposedArgs<CancellationController>> OnDisposed;

        private event EventHandler<SourceChangedArgs<CancellationTokenSource>> OnSourceChanged;

        private CancellationTokenRegistration m_Registration;
        private CancellationTokenSource m_OriginalSource;
        private CancellationTokenSource m_Source;
        private CancellationController m_LinkedTo;

        public bool CancellationRequested => Source.IsCancellationRequested;

        public CancellationTokenSource Source
        {
            get => m_Source;
            private set
            {
                m_Source = value;
                Registation = Token.Register(RequestedCancellation);
            }
        }

        public CancellationToken Token => Source.Token;

        internal CancellationTokenSource OriginalSource
        {
            get => m_OriginalSource;
            private set
            {
                m_OriginalSource = value;
                LinkSourceCorrect();
            }
        }

        private CancellationTokenRegistration Registation
        {
            get => m_Registration;
            set
            {
                _ = m_Registration.Unregister();
                m_Registration = value;
            }
        }

        public CancellationController()
        {
            OriginalSource = CancellationTokenUtil.CreateCancelledSource();
        }

        public void ChangeSource(CancellationTokenSource source)
        {
            var oldSource = Source;
            OriginalSource = source;
            OnSourceChanged?.Invoke(this, new(oldSource, Source));
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this, new(this));
            _ = Registation.Unregister();
            m_OriginalSource?.Dispose();
            m_Source?.Dispose();
        }

        public void LinkTo(CancellationController other)
        {
            if (m_LinkedTo is not null)
            {
                Unlink();
            }
            m_LinkedTo = other;
            LinkSourceCorrect();
            if (other is null)
            {
                return;
            }
            m_LinkedTo.OnDisposed += OnLinkedDisposed;
            m_LinkedTo.OnSourceChanged += OnLinkedSourceChanged;
        }

        public void Reset()
        {
            Source.Cancel();
            OriginalSource = CancellationTokenUtil.CreateCancelledSource();
        }

        public void Unlink()
        {
            m_LinkedTo.OnDisposed -= OnLinkedDisposed;
            m_LinkedTo.OnSourceChanged -= OnLinkedSourceChanged;
            m_LinkedTo = null;
            Source = OriginalSource;
        }

        private void LinkSourceCorrect()
        {
            Source = m_LinkedTo is null
                ? m_OriginalSource
                : CancellationTokenSource.CreateLinkedTokenSource(m_LinkedTo.Token, m_OriginalSource.Token);
        }

        private void OnLinkedDisposed(object? sender, ObjectDisposedArgs<CancellationController> e) => Unlink();

        private void OnLinkedSourceChanged(object? sender, SourceChangedArgs<CancellationTokenSource> e)
        {
            if (e.NewSource is null)
            {
                throw new ArgumentNullException(nameof(e.NewSource));
            }
            if (!e.NewSource.Token.Equals(m_LinkedTo.Token))
            {
                throw new ArgumentException();
            }
            Source = CancellationTokenSource.CreateLinkedTokenSource(m_OriginalSource.Token, e.NewSource.Token);
        }

        private void RequestedCancellation() => OnCancellationRequested?.Invoke();
    }
}