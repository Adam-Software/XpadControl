using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Websocket.Client;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.WebSocketCkientService;

namespace XpadControl.Common.Services.WebSocketCkientService
{
    public class WebSocketClientsService : IWebSocketClientsService
    {
        public event WebSocketConnectedEventHandler RaiseWebSocketConnectedEvent;
        public event WebSocketClientDisconnectEventHandler RaiseWebSocketClientDisconnectedEvent;

        private readonly ILoggerService mLoggerService;
        private readonly WebsocketClient mWebsocketClient;

        public WebSocketClientsService(ILoggerService loggerService, Uri uri) 
        {
            mLoggerService = loggerService;

            mWebsocketClient = new WebsocketClient(uri)
            {
                ReconnectTimeout = null
            };

            StartOrFail();
        }

        public bool IsRunning => mWebsocketClient.IsRunning;

        public bool IsStarted => mWebsocketClient.IsStarted;

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(WebSocketCkientService)} called");

            StopOrFail();
            mWebsocketClient.Dispose();
        }

        public void SendText(string text) => mWebsocketClient.Send(text);

        public Task SendTextInstant(string text) => mWebsocketClient.SendInstant(text);

        public Task StartOrFail() => mWebsocketClient.StartOrFail();

        public Task<bool> StopOrFail() => mWebsocketClient.StopOrFail(WebSocketCloseStatus.NormalClosure, "Nomal close");
    }
}
