using System;
using System.Threading.Tasks;
using XpadControl.JsonModel;

namespace XpadControl.Interfaces.WebSocketCkientService
{
    //public delegate void WebSocketConnectedEventHandler();
    //public delegate void WebSocketClientDisconnectEventHandler();
    public interface IWebSocketClientsService : IDisposable
    {
        //public event WebSocketConnectedEventHandler RaiseWebSocketConnectedEvent;
        //public event WebSocketClientDisconnectEventHandler RaiseWebSocketClientDisconnectedEvent;

        #region IsDisconnection

        public bool WheelClientIsDisconnection { get;  }
        public bool ServosClientIsDisconnection { get; }

        #endregion

        #region Start/Stop/Reconnect

        public Task WheelClientStartOrFail();
        public Task<bool> WheelClientStopOrFail();

        public Task ServosClientStartOrFail();
        public Task<bool> ServosClientStopOrFail();

        public Task WheelClientReconnectOrFail();
        public Task ServosClientReconnectOrFail();

        #endregion

        /// <summary>
        /// Send text with queue
        /// </summary>
        public void Send(string text);

        /// <summary>
        /// Send vector with queue
        /// </summary>
        public void Send(VectorModel vector);

        /// <summary>
        /// Send text without queue
        /// </summary>
        public Task SendInstant(string text);

        /// <summary>
        /// Send vector without queue
        /// </summary>
        public Task SendInstant(VectorModel vector);
    }
}
