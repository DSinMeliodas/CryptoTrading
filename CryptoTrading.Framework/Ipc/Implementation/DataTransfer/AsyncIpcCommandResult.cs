using CryptoTrading.Framework.Ipc.Base.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.Participants;

using System.Runtime.Serialization;
using System.Threading;

namespace CryptoTrading.Framework.Ipc.Implementation.DataTransfer;

[Serializable]
internal sealed class AsyncIpcCommandResult : AsyncIpcCommandResultBase
{
    [NonSerialized]
    public const int TypeCodeConst = 0x0000;

    [NonSerialized]
    private readonly object m_Lock = new();
    [NonSerialized]
    private readonly long m_ResponseTag;

    [NonSerialized]
    private bool m_HasResult;
    [NonSerialized]
    private object? m_Result;


    public override object ResultData
    {
        get
        {
            lock (m_Lock)
            {
                if (HasResult)
                {
                    return m_Result;
                }
                AwaitResult();
                return m_Result;
            }

        }
        protected set
        {
            lock (m_Lock)
            {
                m_Result = value;
                m_HasResult = true;
            }
        }
    }

    public override bool HasResult
    {
        get
        {
            lock (m_Lock)
            {
                return m_HasResult;
            }
        }
    }

    private AsyncIpcCommandResult(long responseTag) : base(TypeCodeConst)
    {
        m_ResponseTag = responseTag;
    }

    public AsyncIpcCommandResult(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    private void AwaitResult() => SpinWait.SpinUntil(ReceivedCommand);

    private void OnResultReceived<TListenerTarget>(IIpcListener<TListenerTarget> sender, IIpcCommand command)
        where TListenerTarget : IIpcListenerTarget
    {
        if (command?.ResponseTag != m_ResponseTag)
        {
            return;
        }
        TakeOver(command.Result);
        sender.OnCommandReceived -= OnResultReceived;
    }

    private bool ReceivedCommand() => HasResult;

    private void TakeOver(IIpcCommandResult result)
    {
        Error = result.Error;
        ResultType = result.ResultType;
        ResultData = result.ResultData;
    }

    public static IAsyncIpcCommandResult CreateLinkFor<T>(IIpcCommand command, IIpcListener<T> listener)
        where T : IIpcListenerTarget
    {
        AsyncIpcCommandResult result = new(command.ResponseTag);
        listener.OnCommandReceived += result.OnResultReceived;
        return result;
    }
}