namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    /// <summary>
    /// Represents a target that an <see cref="IIpcListener{TListenerTarget}"/> can listen to.
    /// </summary>
    internal interface IIpcListenerTarget
    {
        /// <summary>
        /// The address of the target.
        /// </summary>
        string Address { get; }
    }
}