using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.WebSocketCkientService;

namespace XpadControl.Common.Services.WebSocketService
{
    public class WebSocketClientsHostedService : BackgroundService
    {
        private readonly IWebSocketClientsService mClientsService;
        public WebSocketClientsHostedService(IWebSocketClientsService webSocketClientsService) 
        {
            mClientsService = webSocketClientsService;

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

                     await Task.Delay(2000);
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
