using CryptoTrading.Framework.Ipc.Interface.Participants;
using CryptoTrading.Framework.Ipc.Interface.Serialization;

using System;
using System.IO;
using System.Threading;

namespace CryptoTrading.Framework.Ipc.Base.Clients
{
    internal abstract class AbstractStreamIpcClient<TListenerTarget> : AbstractBaseIpcClient<TListenerTarget>
        where TListenerTarget : IIpcListenerTarget
    {
        private CancellationTokenSource m_CancellationSource;

        protected CancellationToken CancellationToken => m_CancellationSource?.Token ?? new CancellationToken(true);

        protected IIpcCommandDeserializer Deserializer { get; }

        protected IIpcCommandSerializer Serializer { get; }

        protected Stream Stream { get; }

        protected AbstractStreamIpcClient(
            TimeSpan updateTickRate,
            Stream underlyingStream,
            IIpcCommandDeserializer deserializer,
            IIpcCommandSerializer serializer)
            : base(updateTickRate)
        {
            Deserializer = deserializer;
            Serializer = serializer;
            Stream = underlyingStream;
        }

        protected sealed override void UpdateTick(object _)
        {
            var buffer = new byte[Deserializer.BufferSize];
            _ = Stream.Read(buffer);
            var command = Deserializer.Deserialize(buffer);
            ReceiveCommand(command);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
            {
                return;
            }
            Stream?.Dispose();
            m_CancellationSource?.Dispose();
        }
    }
}