using System;
using System.Threading.Tasks;

namespace XpadControl.Interfaces.WebSocketCkientService
{
    public delegate void WebSocketConnectedEventHandler();
    public delegate void WebSocketClientDisconnectEventHandler();
    public interface IWebSocketClientsService : IDisposable
    {
        public event WebSocketConnectedEventHandler RaiseWebSocketConnectedEvent;
        public event WebSocketClientDisconnectEventHandler RaiseWebSocketClientDisconnectedEvent;

        public bool IsRunning { get; }
        public bool IsStarted { get; }
        public Task StartOrFail();
        public Task<bool> StopOrFail();

        /// <summary>
        /// send text without queue
        /// </summary>
        public void SendText(string text);

        /// <summary>
        /// send text without queue
        /// </summary>
        public Task SendTextInstant(string text);
    }
}
