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

        #region IsRunning/IsStarted

        public bool WheelClientIsRunning { get; }
        public bool WheelClientIsStarted { get; }
        
        public bool ServosClientIsRunning { get; }
        public bool ServosClientIsStarted { get; }

        #endregion

        #region Start/Stop

        public Task WheelClientStartOrFail();
        public Task<bool> WheelClientStopOrFail();

        public Task ServosClientStartOrFail();
        public Task<bool> ServosClientStopOrFail();

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
