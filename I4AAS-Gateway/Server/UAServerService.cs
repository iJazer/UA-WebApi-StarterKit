using I4AAS_Gateway.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace I4AAS_Gateway.Server
{
    public class UAServerService : BackgroundService
    {
        private readonly ILogger<UAServerService> Log;
        private readonly IMemoryCache m_cache;
        private AASServer m_server;

        public UAServerService(ILogger<UAServerService> logger, IMemoryCache cache)
        {
            Log = logger;
            m_cache = cache;
            m_server = new AASServer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await m_server.Start(Log, stoppingToken).ConfigureAwait(false);
                m_cache.Set(nameof(UAServerService), m_server.InternalClient, TimeSpan.FromDays(365));
            }
            catch (Exception e)
            {
                Log.LogError(e, "Error starting OPC UA server.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

            try
            {
                await m_server.Stop().ConfigureAwait(false);
                m_cache.Remove(nameof(UAServerService));
            }
            catch (Exception e)
            {
                Log.LogError(e, "Error stopping OPC UA server.");
                return;
            }
        }
    }
}
