using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

namespace CryptoTrading.Framework.Ipc.Interface.Serialization
{
    /// <summary>
    /// Describes a method to deserialize a byte sequence back to an <see cref="IIpcCommand"/>.
    /// </summary>
    public interface IIpcCommandDeserializer
    {
        /// <summary>
        /// The size of the buffer used by the deserializer.
        /// </summary>
        int BufferSize { get; }

        /// <summary>
        /// Deserializes the byte sequence back to the transmitted command.
        /// </summary>
        /// <param name="commandBuffer">The byte sequence to be deserialized.</param>
        /// <returns>The resulting command</returns>
        IIpcCommand Deserialize(byte[] commandBuffer);
    }
}