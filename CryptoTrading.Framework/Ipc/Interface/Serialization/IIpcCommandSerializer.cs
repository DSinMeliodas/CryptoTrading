using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

namespace CryptoTrading.Framework.Ipc.Interface.Serialization
{
    public interface IIpcCommandSerializer
    {
        byte[] Serialize(IIpcCommand command);
    }
}