using CryptoTrading.Framework.Ipc.Base.Clients;
using CryptoTrading.Framework.Ipc.Interface.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.Participants;
using CryptoTrading.Framework.Ipc.Interface.Serialization;

using System;
using System.IO.Pipes;
using CryptoTrading.Framework.Ipc.Implementation.DataTransfer;

namespace CryptoTrading.Framework.Ipc.Implementation.Clients
{
    internal sealed class NamedPipeServerIpcClient : AbstractStreamIpcClient<IIpcListenerTarget>
    {
        public NamedPipeServerStream NamedStream => Stream as NamedPipeServerStream;

        public NamedPipeServerIpcClient(
            TimeSpan updateTickRate,
            NamedPipeServerStream serverStream,
            IIpcCommandDeserializer deserializer,
            IIpcCommandSerializer serializer)
            : base(updateTickRate, serverStream, deserializer, serializer)
        {
        }

        public override bool StartListening()
        {
            if (!base.StartListening())
            {
                return false;
            }
            NamedStream.WaitForConnectionAsync(CancellationToken).Wait();
            return true;
        }

        public override IIpcCommandResult Send(IIpcCommand command)
        {
            if (!NamedStream.IsConnected)
            {
                NamedStream.WaitForConnectionAsync(CancellationToken).Wait();
            }
            var rawCommand = Serializer.Serialize(command);
            NamedStream.WriteAsync(rawCommand, CancellationToken).AsTask().Wait();
            return AsyncIpcCommandResult.CreateLinkFor(command, this);
        }
    }
}