using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface.DataTransfer
{
    public interface IIpcCommand : ISerializable
    {
        string Id { get; }

        long ResponseTag { get; }

        IIpcCommandResult Result { get; set; }
    }
}