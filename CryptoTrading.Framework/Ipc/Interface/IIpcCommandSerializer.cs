namespace CryptoTrading.Framework.Ipc.Interface
{
    public interface IIpcCommandSerializer
    {
        byte[] Serialize(IIpcCommand command);
    }
}