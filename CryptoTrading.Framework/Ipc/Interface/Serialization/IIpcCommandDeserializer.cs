using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

namespace CryptoTrading.Framework.Ipc.Interface.Serialization
{
    public interface IIpcCommandDeserializer
    {
        int BufferSize { get; }

        IIpcCommand Deserialize(byte[] commandBuffer);
    }
}