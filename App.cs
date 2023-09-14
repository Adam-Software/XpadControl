using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.WebSocketCkientService;
using XpadControl.JsonModel;

namespace XpadControl
{
    public class App : IHostedService, IDisposable
    {
        private readonly ILoggerService mLoggerService;
        private readonly IWebSocketClientsService mWebSocketClientsService;
        private readonly IGamepadService mGamepadService;
        private readonly IHostApplicationLifetime mApplicationLifetime;

        public App(IWebSocketClientsService webSocketClientsService, ILoggerService loggerService, IGamepadService gamepadService, 
            IHostApplicationLifetime applicationLifetime)
        {
            mLoggerService = loggerService;
            mWebSocketClientsService = webSocketClientsService;
            mGamepadService = gamepadService;
            mApplicationLifetime = applicationLifetime;

            mApplicationLifetime.ApplicationStarted.Register(OnStarted);
            mApplicationLifetime.ApplicationStopped.Register(OnStopped);
            mApplicationLifetime.ApplicationStopping.Register(OnStopping);

            mGamepadService.RaiseButtonChangedEvent += RaiseButtonChangedEvent;
            mGamepadService.RaiseAxisChangedEvent += RaiseAxisChangedEvent;

            mGamepadService.RaiseLeftTriggerChangedEvent += RaiseLeftTriggerChangedEvent;
            mGamepadService.RaiseRightTriggerChangedEvent += RaiseRightTriggerChangedEvent;

            mGamepadService.RaiseButtonChangedEvent += RaiseButtonChangedEvent;
        }

        #region Gamepad event

        #region Trigger event

        private void RaiseRightTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"RaiseRightTriggerChangedEvent {e.Value}");
        }

        private void RaiseLeftTriggerChangedEvent(object sender, TriggerEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"RaiseLeftTriggerChangedEvent {e.Value}");
        }

        #endregion

        #region Axis event

        private void RaiseAxisChangedEvent(object sender, AxisEventArgs left, AxisEventArgs right)
        {
            VectorModel vector = new()
            {
                Move = new VectorItem
                {
                    X = left.X,
                    Y = left.Y,
                    Z = right.X
                }
            };

            mWebSocketClientsService.SendTextInstant(vector);

            mLoggerService.WriteDebugLog($"lx {left.X} ly {left.Y} rx {right.X} ry {right.Y}");
        }

        #endregion

        #region Button event

        private void RaiseButtonChangedEvent(object sender, ButtonEventArgs e)
        {
            //mLoggerService.WriteVerboseLog($"RaiseButtonChangedEvent {e.Button} is {e.Pressed}");
        }

        #endregion

        #endregion

        #region App event 

        private void OnStopping()
        {
            mLoggerService.WriteVerboseLog("OnStopping() called");
        }

        private void OnStopped()
        {
            mLoggerService.WriteVerboseLog("OnStopped() called");
        }

        private void OnStarted()
        {
            mLoggerService.WriteVerboseLog("OnStarted() called");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            mLoggerService.WriteInformationLog("App start. Try use gamepad");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            mLoggerService.WriteVerboseLog("Task complete and stop async called");
            return Task.CompletedTask;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose all services
        /// </summary>
        public void Dispose()
        {
            mLoggerService.WriteVerboseLog("Coomon dispose called");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                mLoggerService.Dispose();
                mWebSocketClientsService.Dispose();
                mGamepadService.Dispose();
            }
        }

        #endregion
    }
}
