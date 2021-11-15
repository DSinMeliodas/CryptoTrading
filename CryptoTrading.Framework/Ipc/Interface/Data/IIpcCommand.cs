using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface.Data
{
    public interface IIpcCommand : ISerializable
    {
        string Id { get; }

        long ResponseTag { get; }

        IIpcCommandResult Result { get; set; }
    }
}