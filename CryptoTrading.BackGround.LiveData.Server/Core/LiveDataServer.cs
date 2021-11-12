using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTrading.BackGround.LiveData.Server.Core
{
    public class LiveDataServer : BackgroundService
    {
        private readonly ILogger<LiveDataServer> m_Logger;

        public LiveDataServer(ILogger<LiveDataServer> logger)
        {
            m_Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                m_Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
