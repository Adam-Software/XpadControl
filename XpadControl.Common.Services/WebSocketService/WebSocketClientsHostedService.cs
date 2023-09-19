using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces;
using XpadControl.Interfaces.Common.Dependencies.SettingsCollection;

namespace XpadControl.Common.Services.WebSocketService
{
    public class WebSocketClientsHostedService : BackgroundService
    {
        private readonly IWebSocketClientsService mClientsService;
        private readonly int mWebsocketReconnectInterval;

        public WebSocketClientsHostedService(IWebSocketClientsService webSocketClientsService, UpdateIntervalCollection updateIntervalCollection) 
        {
            mClientsService = webSocketClientsService;
            mWebsocketReconnectInterval = updateIntervalCollection.WebsocketReconnectInterval;

            mClientsService.WheelClientStartOrFail();
            mClientsService.ServosClientStartOrFail();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (mClientsService.WheelClientIsDisconnection)
                        WheelClientReconnectOrFail();

                    if (mClientsService.ServosClientIsDisconnection)
                        ServosClientReconnectOrFail();

                     await Task.Delay(mWebsocketReconnectInterval);
                }

            }, stoppingToken);

            return Task.CompletedTask;
        }

        private void WheelClientReconnectOrFail()
        {
            mClientsService.WheelClientReconnectOrFail();
        }

        private void ServosClientReconnectOrFail()
        {
            mClientsService.ServosClientReconnectOrFail();
        }
    }
}
