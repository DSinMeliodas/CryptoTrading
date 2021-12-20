using CryptoTrading.Framework.Ipc.Base.Clients;
using CryptoTrading.Framework.Ipc.Implementation.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.Participants;
using CryptoTrading.Framework.Ipc.Interface.Serialization;

using System.IO.Pipes;

namespace CryptoTrading.Framework.Ipc.Implementation.Clients;

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

    public override IIpcCommandResult Send(IIpcCommand command)
    {
        if (!NamedStream.IsConnected)
        {
            NamedStream.WaitForConnectionAsync(RunningToken).Wait();
        }
        var rawCommand = Serializer.Serialize(command);
        NamedStream.WriteAsync(rawCommand, RunningToken).AsTask().Wait();
        return AsyncIpcCommandResult.CreateLinkFor(command, this);
    }

    protected override bool OnListeningStart() => NamedStream.WaitForConnectionAsync(ListeningToken).IsCompletedSuccessfully;

    protected override bool OnListeningStop()
    {
        NamedStream.Disconnect();
        return true;
    }

    protected override bool OnStart() => true;

    protected override bool OnStop() => true;
}