using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using Websocket.Client;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.WebSocketCkientService;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;
using XpadControl.JsonModel;

namespace XpadControl.Common.Services.WebSocketCkientService
{
    public class WebSocketClientsService : IWebSocketClientsService
    {
        //public event WebSocketConnectedEventHandler RaiseWebSocketConnectedEvent;
        //public event WebSocketClientDisconnectEventHandler RaiseWebSocketClientDisconnectedEvent;

        private readonly ILoggerService mLoggerService;
        private readonly WebsocketClient mWheelWebsocketClient;
        private readonly WebsocketClient mServosWebsocketClient;

        public WebSocketClientsService(ILoggerService loggerService, UriCollection uri) 
        {
            mLoggerService = loggerService;

            mWheelWebsocketClient = new WebsocketClient(uri.WheelWebSocketUri)
            {
                ReconnectTimeout = null
            };

            mServosWebsocketClient = new WebsocketClient(uri.ServosWebSocketUri)
            {
                ReconnectTimeout = null
            };

            mWheelWebsocketClient.ReconnectionHappened.Subscribe(WheelClientReconectHappened);
            mWheelWebsocketClient.DisconnectionHappened.Subscribe(WheelClientDisconnectionHappened);

            mServosWebsocketClient.ReconnectionHappened.Subscribe(ServosClientReconectHappened);
            mServosWebsocketClient.DisconnectionHappened.Subscribe(ServosClientDisconnectionHappened);

            WheelClientStartOrFail();
            ServosClientStartOrFail();
        }

        #region client events 

        private void WheelClientDisconnectionHappened(DisconnectionInfo info)
        {
            mLoggerService.WriteInformationLog($"Wheel websocket client disconnection happened reason {info.Type}");
        }

        private void WheelClientReconectHappened(ReconnectionInfo info)
        {
            mLoggerService.WriteInformationLog($"Wheel websocket client reconect happened reason {info.Type}");
        }

        private void ServosClientDisconnectionHappened(DisconnectionInfo info)
        {
            mLoggerService.WriteInformationLog($"Servos websocket client disconnection happened reason {info.Type}");
        }

        private void ServosClientReconectHappened(ReconnectionInfo info)
        {
            mLoggerService.WriteInformationLog($"Servos websocket client reconect happened reason {info.Type}");
        }

        #endregion

        #region IsRunning/IsStarted

        /// <summary>
        /// If connect to server true, false otherwise
        /// </summary>
        public bool WheelClientIsRunning => mWheelWebsocketClient.IsRunning;

        public bool WheelClientIsStarted => mWheelWebsocketClient.IsStarted;

        /// <summary>
        /// If connect to server true, false otherwise
        /// </summary>
        public bool ServosClientIsRunning => mServosWebsocketClient.IsRunning;

        public bool ServosClientIsStarted => mServosWebsocketClient.IsStarted;

        #endregion

        #region Start/Stop

        public Task WheelClientStartOrFail() => mWheelWebsocketClient.StartOrFail();

        public Task<bool> WheelClientStopOrFail() => mWheelWebsocketClient.StopOrFail(WebSocketCloseStatus.NormalClosure, "Nomal close");

        public Task ServosClientStartOrFail() => mServosWebsocketClient.StartOrFail();

        public Task<bool> ServosClientStopOrFail() => mServosWebsocketClient.StopOrFail(WebSocketCloseStatus.NormalClosure, "Nomal close");

        #endregion

        public void SendText(string text) => mWheelWebsocketClient.Send(text);

        public void SendText(VectorModel vector)
        {
            string json = JsonSerializer.Serialize(vector);
            mWheelWebsocketClient.Send(json);
        }

        public Task SendTextInstant(string text) => mWheelWebsocketClient.SendInstant(text);

        public Task SendTextInstant(VectorModel vector) 
        {
            string json = JsonSerializer.Serialize(vector);
            return mWheelWebsocketClient.SendInstant(json);
        }

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(WebSocketClientsService)} called");

            WheelClientStopOrFail();
            ServosClientStopOrFail();

            mWheelWebsocketClient.Dispose();
            mServosWebsocketClient.Dispose();
        }
    }
}
