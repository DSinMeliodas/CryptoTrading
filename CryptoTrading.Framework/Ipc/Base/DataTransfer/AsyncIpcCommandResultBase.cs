using CryptoTrading.Framework.Ipc.Interface.DataTransfer;

using System;
using System.Runtime.Serialization;

namespace CryptoTrading.Framework.Ipc.Base.DataTransfer
{
    [Serializable]
    internal abstract class AsyncIpcCommandResultBase : IAsyncIpcCommandResult
    {
        public int TypeCode { get; }

        public bool Error { get; protected set; }

        public Type ResultType { get; protected set; }

        public abstract bool HasResult { get; }

        public abstract object ResultData { get; protected set; }

        protected AsyncIpcCommandResultBase(int typeCode)
        {
            TypeCode = typeCode;
        }

        protected AsyncIpcCommandResultBase(SerializationInfo info, StreamingContext context)
        {
            TypeCode = info.GetInt32(nameof(TypeCode));
            Error = info.GetBoolean(nameof(Error));
            ResultType = info.GetValue(nameof(ResultType), typeof(Type)) as Type ?? throw new SerializationException();
            ResultData = info.GetValue(nameof(ResultData), ResultType);
        }

        public T CastResultData<T>()
        {
            if (typeof(T).IsAssignableFrom(ResultType))
            {
                throw new NotSupportedException();
            }

            return (T)ResultData;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (ResultType is null)
            {
                throw new ArgumentNullException(nameof(ResultType));
            }
            info.AddValue(nameof(TypeCode), TypeCode);
            info.AddValue(nameof(Error), Error);
            info.AddValue(nameof(ResultType), ResultType);
            info.AddValue(nameof(ResultData), ResultData, ResultType);
        }
    }
}