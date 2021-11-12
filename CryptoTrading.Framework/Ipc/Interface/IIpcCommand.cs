using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface
{
    public interface IIpcCommand : ISerializable
    {
        int Id { get; }
    }
}