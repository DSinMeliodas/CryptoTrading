using CryptoTrading.Framework.Ipc.Interface.Data;

namespace CryptoTrading.Framework.Ipc.Interface.Serialization
{
    public interface IIpcCommandSerializer
    {
        byte[] Serialize(IIpcCommand command);
    }
}