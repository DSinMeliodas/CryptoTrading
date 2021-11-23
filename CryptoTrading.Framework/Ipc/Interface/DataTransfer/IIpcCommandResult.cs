using System;
using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Interface.DataTransfer
{
    public interface IIpcCommandResult : ISerializable
    {
        bool Error { get; }

        int TypeCode { get; }

        object ResultData { get; }

        Type ResultType { get; }

        T CastResultData<T>();
    }
}