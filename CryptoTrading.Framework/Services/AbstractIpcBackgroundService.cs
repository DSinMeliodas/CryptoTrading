using CryptoTrading.Framework.Ipc.Interface.DataTransfer;
using CryptoTrading.Framework.Ipc.Interface.Participants;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace CryptoTrading.Framework.Services;

internal abstract class AbstractIpcBackgroundService<TListenerTarget> : BackgroundService where TListenerTarget : IIpcListenerTarget
{
    private bool m_Disposed;

    private readonly IIpcClient<TListenerTarget> m_IpcClient;

    protected ILogger Logger { get; }

    protected AbstractIpcBackgroundService(ILogger logger, IIpcClient<TListenerTarget> ipcClient)
    {
        m_IpcClient = ipcClient;
        Logger = logger;
        m_IpcClient.OnCommandReceived += ReceivedCommand;
    }

    public sealed override void Dispose()
    {
        base.Dispose();
        CleanUp();
        Dispose(!m_Disposed);
        m_Disposed = true;
    }

    protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoLoopWork(stoppingToken);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    protected abstract Task DoLoopWork(CancellationToken stoppingToken);

    protected abstract void ReceivedCommand(IIpcListener<TListenerTarget> sender, IIpcCommand command);

    private void CleanUp()
    {
        if (m_IpcClient is null)
        {
            return;
        }
        m_IpcClient.Dispose();
        m_IpcClient.OnCommandReceived -= ReceivedCommand;
    }
}