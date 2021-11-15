using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface.Data
{
    public interface IIpcCommandResult : ISerializable
    {
        bool Error { get; }

        int Code { get; }

        object ResultData { get; }
    }
}