using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

namespace CryptoTrading.Framework.Ipc.Interface.Serialization;

/// <summary>
/// Describes a method to serialize an <see cref="IIpcCommand"/> into a byte sequence.
/// </summary>
public interface IIpcCommandSerializer
{
    /// <summary>
    /// Serializes a command into a sequence of bytes.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    byte[] Serialize(IIpcCommand command);
}