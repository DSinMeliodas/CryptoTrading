namespace CryptoTrading.Framework.Ipc.Interface.Participants
{
    internal interface IIpcClient<TListenerTarget> : IIpcListener<TListenerTarget>, IIpcSender
        where TListenerTarget: IIpcListenerTarget
    {
        void Start();

        void Stop();
    }
}