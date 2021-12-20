using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface.DataTransfer;

/// <summary>
/// Represents a command that is send between processes.
/// </summary>
public interface IIpcCommand : ISerializable
{
    /// <summary>
    /// The unique id for identifying this specific command type.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The response tag to identify the corresponding result.
    /// </summary>
    long ResponseTag { get; }

    /// <summary>
    /// The result of the command.
    /// </summary>
    IIpcCommandResult Result { get; set; }
}