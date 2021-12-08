using System;
using System.Runtime.Serialization;
using CryptoTrading.Framework.Ipc.Interface.Participants;

namespace CryptoTrading.Framework.Ipc.Interface.DataTransfer
{
    /// <summary>
    /// Represents the result of a command that has been send by a <see cref="IIpcSender"/>
    /// </summary>
    public interface IIpcCommandResult : ISerializable
    {
        /// <summary>
        /// Bool value indicating whether an error has occoured.
        /// </summary>
        bool Error { get; }

        /// <summary>
        /// A value that can be used to identify the type of result.<br/>
        /// e.g: error, success, other possible results
        /// </summary>
        int TypeCode { get; }

        /// <summary>
        /// The data of the result.
        /// </summary>
        object ResultData { get; }

        /// <summary>
        /// The type of <see cref="ResultData"/>
        /// </summary>
        Type ResultType { get; }

        /// <summary>
        /// Casts the <see cref="ResultData"/> to the given type if <see cref="ResultType"/> is the same
        /// type or derived from it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CastResultData<T>();
    }
}