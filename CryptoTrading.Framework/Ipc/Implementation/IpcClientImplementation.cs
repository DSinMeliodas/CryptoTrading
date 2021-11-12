using CryptoTrading.Framework.Ipc.Interface;

namespace CryptoTrading.Framework.Ipc.Implementation
{
    internal sealed class IpcClientImplementation : IIpcClient<IIpcListenerTarget>
    {
        public event IpcCommandReceived OnCommandReceived;

        public IIpcListenerTarget Target { get; init; }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IIpcCommandResult Send(IIpcCommand command)
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public bool StartListening()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public bool StopListening()
        {
            throw new System.NotImplementedException();
        }
    }
}