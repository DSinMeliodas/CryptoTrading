using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface
{
    public interface IIpcCommandResult : ISerializable
    {
        bool Error { get; }

        int Code { get; }
    }
}