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
                ReconnectTimeout = null,
                 
            };

            mWebsocketClient.ReconnectionHappened.Subscribe(ReconectHappened);
            mWebsocketClient.DisconnectionHappened.Subscribe(DisconnectionHappened);

            StartOrFail();

            mLoggerService.WriteInformationLog($"Connect with uri {uri.AbsoluteUri} result {IsRunning}");
        }

        #region client events 

        private void DisconnectionHappened(DisconnectionInfo info)
        {
            mLoggerService.WriteInformationLog($"DisconnectionHappened reason {info.Type}");
        }

        private void ReconectHappened(ReconnectionInfo info)
        {
            mLoggerService.WriteInformationLog($"ReconectHappened reason {info.Type}");
        }

        #endregion

        /// <summary>
        /// If connect to server true, false otherwise
        /// </summary>
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
