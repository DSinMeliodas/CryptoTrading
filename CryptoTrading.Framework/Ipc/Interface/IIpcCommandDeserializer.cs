namespace CryptoTrading.Framework.Ipc.Interface
{
    public interface IIpcCommandDeserializer
    {
        int BufferSize { get; }

        IIpcCommand Deserialize(byte[] commandBuffer);
    }
}