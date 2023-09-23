using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;
using Websocket.Client;
using XpadControl.Interfaces;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.EventArgs;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel;
using XpadControl.JsonModel;

namespace XpadControl.Common.Services.WebSocketService
{
    public class WebSocketClientsService : IWebSocketClientsService
    {
        public event IsDisconnectionStatusChangedEventHandler RaiseIsDisconnectionStatusChangedEvent;

        private readonly ILoggerService mLoggerService;
        private readonly WebsocketClient mWheelWebsocketClient;
        private readonly WebsocketClient mServosWebsocketClient;

        public WebSocketClientsService(ILoggerService loggerService, UriCollection uri) 
        {
            mLoggerService = loggerService;

            mWheelWebsocketClient = new WebsocketClient(uri.WheelWebSocketUri)
            {
                ReconnectTimeout = null,
                ErrorReconnectTimeout = null,
                Name = "WheelWebsocketClient"

            };

            mServosWebsocketClient = new WebsocketClient(uri.ServosWebSocketUri)
            {
                ReconnectTimeout = null,
                ErrorReconnectTimeout = null,
                Name = "ServosWebsocketClient"
            };

            mWheelWebsocketClient.ReconnectionHappened.Subscribe(WheelClientReconectHappened);
            mWheelWebsocketClient.DisconnectionHappened.Subscribe(WheelClientDisconnectionHappened);

            mServosWebsocketClient.ReconnectionHappened.Subscribe(ServosClientReconectHappened);
            mServosWebsocketClient.DisconnectionHappened.Subscribe(ServosClientDisconnectionHappened);
        }

        #region Client events 

        private void WheelClientDisconnectionHappened(DisconnectionInfo info)
        {
            WheelClientIsDisconnection = true;
            
            mLoggerService.WriteVerboseLog($"Wheel websocket client disconnection happened reason {info.Type}");
        }

        private void WheelClientReconectHappened(ReconnectionInfo info)
        {
            WheelClientIsDisconnection = false;
            
            mLoggerService.WriteVerboseLog($"Wheel websocket client reconect happened reason {info.Type}");
        }

        private void ServosClientDisconnectionHappened(DisconnectionInfo info)
        {
            ServosClientIsDisconnection = true;

            mLoggerService.WriteVerboseLog($"Servos websocket client disconnection happened reason {info.Type}");
        }

        private void ServosClientReconectHappened(ReconnectionInfo info)
        {
            ServosClientIsDisconnection = false;

            mLoggerService.WriteVerboseLog($"Servos websocket client reconect happened reason {info.Type}");
        }

        #endregion

        #region IsDisconnection

        // null because when initializing the service,
        // the event is not triggered because the check occurs: new value == current value
        private bool? mWheelClientIsDisconnection = null;
        public bool WheelClientIsDisconnection 
        { 
            get 
            {
                switch (mWheelClientIsDisconnection)
                {
                    case false:
                        return false;
                    case true:
                        return true;
                    case null:
                        return false;
                }
            }

            private set
            {
                if (mWheelClientIsDisconnection == value)
                    return; 
                
                mWheelClientIsDisconnection = value;

                OnRaiseIsDisconnectionStatusChangedEvent(mWheelWebsocketClient.Name, WheelClientIsDisconnection);
            } 
        }

        // null because when initializing the service,
        // the event is not triggered because the check occurs: new value == current value
        private bool? mServosClientIsDisconnection = null;
        public bool ServosClientIsDisconnection 
        { 
            get 
            {
                switch (mServosClientIsDisconnection)
                {
                    case false:
                        return false;
                    case true:
                        return true;
                    case null:
                        return false;
                }
            }

            private set
            {
                if(mServosClientIsDisconnection == value)
                    return;

                mServosClientIsDisconnection = value;

                OnRaiseIsDisconnectionStatusChangedEvent(mServosWebsocketClient.Name, ServosClientIsDisconnection);
            } 
        }

        #endregion

        #region Start/Stop/Reconnect

        public Task WheelClientStartOrFail()
        {
            mLoggerService.WriteVerboseLog("Try to connect wheel clients ...");
            return mWheelWebsocketClient.StartOrFail();
        }

        public Task WheelClientReconnectOrFail()
        {
            mLoggerService.WriteVerboseLog("Try to reconnect wheel clients ...");
            return mWheelWebsocketClient.ReconnectOrFail();
        }

        public Task<bool> WheelClientStopOrFail() => mWheelWebsocketClient.StopOrFail(WebSocketCloseStatus.NormalClosure, "Nomal close");

        public Task ServosClientStartOrFail() 
        {
            mLoggerService.WriteVerboseLog("Try to connect servo clients ...");
            return mServosWebsocketClient.StartOrFail(); 
        }

        public Task ServosClientReconnectOrFail()
        {
            mLoggerService.WriteVerboseLog("Try to reconnect servo clients ...");
            return mServosWebsocketClient.ReconnectOrFail();
        }

        public Task<bool> ServosClientStopOrFail() => mServosWebsocketClient.StopOrFail(WebSocketCloseStatus.NormalClosure, "Nomal close");

        #endregion

        #region Send/SendInstant

        public void Send(string text) => mWheelWebsocketClient.Send(text);

        public void Send(Vector vector)
        {
            string json = JsonSerializer.Serialize(vector);
            mWheelWebsocketClient.Send(json);
        }

        public void Send(ServoCommands commands)
        {
            string json = JsonSerializer.Serialize(commands);
            mServosWebsocketClient.Send(json);
        }

        public Task SendInstant(string text) => mWheelWebsocketClient.SendInstant(text);

        public Task SendInstant(Vector vector) 
        {
            string json = JsonSerializer.Serialize(vector);
            return mWheelWebsocketClient.SendInstant(json);
        }

        public Task SendInstant(ServoCommands servoCommands)
        {
            string json = JsonSerializer.Serialize(servoCommands);
            return mServosWebsocketClient.SendInstant(json);
        }

        #endregion

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(WebSocketClientsService)} called");

            if (mServosWebsocketClient.IsRunning)
                ServosClientStopOrFail();

            if (mWheelWebsocketClient.IsRunning)
                WheelClientStopOrFail();

            if (mServosWebsocketClient.IsStarted)
                mServosWebsocketClient.Dispose();

            if (mWheelWebsocketClient.IsStarted)
                mWheelWebsocketClient.Dispose();
        }

        protected virtual void OnRaiseIsDisconnectionStatusChangedEvent(string clientName, bool isDisconnection)
        {
            IsDisconnectionStatusChangedEventHandler raiseEvent = RaiseIsDisconnectionStatusChangedEvent;

            IsDisconnectionStatusChangedEventArgs eventArgs = new IsDisconnectionStatusChangedEventArgs()
            {
                 ClientName = clientName,
                 IsDisconnection = isDisconnection
            };

            raiseEvent?.Invoke(this, eventArgs);
        }
    }
}
