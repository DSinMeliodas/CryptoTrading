namespace CryptoTrading.Framework.Ipc.Interface
{
    internal interface IIpcClient<TListenerTarget> : IIpcListener<TListenerTarget>, IIpcSender
        where TListenerTarget: IIpcListenerTarget
    {
        void Start();

        void Stop();
    }
}