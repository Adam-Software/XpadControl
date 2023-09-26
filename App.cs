using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.EventArgs;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel;
using XpadControl.JsonModel;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Extensions;

namespace XpadControl
{
    public class App : IHostedService, IDisposable
    {
        private readonly ILoggerService mLoggerService;
        private readonly IWebSocketClientsService mWebSocketClientsService;
        private readonly IGamepadService mGamepadService;
        private readonly PathCollection mAppArguments;

        public App(IWebSocketClientsService webSocketClientsService, ILoggerService loggerService, IGamepadService gamepadService, 
            IBindingButtonsService bindingButtonsService, IHostApplicationLifetime applicationLifetime, PathCollection appArguments)
        {
            mLoggerService = loggerService;
            mWebSocketClientsService = webSocketClientsService;
            mGamepadService = gamepadService;
            mAppArguments = appArguments;

            applicationLifetime.ApplicationStarted.Register(OnStarted);
            applicationLifetime.ApplicationStopped.Register(OnStopped);
            applicationLifetime.ApplicationStopping.Register(OnStopping);

            SubscribeToEvent();
        }

        #region Websocket clients event

        private void RaiseIsDisconnectionStatusChangedEvent(object sender, IsDisconnectionStatusChangedEventArgs eventArgs)
        {
            mLoggerService.WriteInformationLog($"{eventArgs.ClientName} is connected status now is {!eventArgs.IsDisconnection}");
        }

        #endregion

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
            Vector vector = new()
            {
                Move = new VectorItem
                {
                    X = left.X,
                    Y = left.Y,
                    Z = right.X
                }
            };

            mWebSocketClientsService.SendInstant(vector);

            mLoggerService.WriteDebugLog($"lx {left.X} ly {left.Y} rx {right.X} ry {right.Y}");
        }

        #endregion

        #region Button event

        private void RaiseButtonChangedEvent(object sender, ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"RaiseButtonChangedEvent in app {e.Button} is {e.Pressed}");

            switch (e.Button)
            {
                case Buttons.Back:

                    if (e.Pressed)
                    {
                        ServoCommands zeroPosition = mAppArguments.ZeroPositionConfigPath.ToServoCommands();
                        mWebSocketClientsService.SendInstant(zeroPosition);
                    }
                    
                    break;
            }
        }

        #endregion

        #region Connect/disconnect gamepad event

        private void RaiseConnectedChangedEvent(object sender, ConnectedEventArgs e)
        {
            mLoggerService.WriteInformationLog($"Change status gamepad connection. Now connection is {e.IsConnected}");
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

        #region Subscribe/Unsubscribe to/from event

        private void SubscribeToEvent()
        {
            mGamepadService.RaiseAxisChangedEvent += RaiseAxisChangedEvent;
            mGamepadService.RaiseLeftTriggerChangedEvent += RaiseLeftTriggerChangedEvent;
            mGamepadService.RaiseRightTriggerChangedEvent += RaiseRightTriggerChangedEvent;
            mGamepadService.RaiseButtonChangedEvent += RaiseButtonChangedEvent;
            mGamepadService.RaiseConnectedChangedEvent += RaiseConnectedChangedEvent;

            mWebSocketClientsService.RaiseIsDisconnectionStatusChangedEvent += RaiseIsDisconnectionStatusChangedEvent;
        }

        private void UnsubscribeFromEvent()
        {
            mGamepadService.RaiseAxisChangedEvent -= RaiseAxisChangedEvent;
            mGamepadService.RaiseLeftTriggerChangedEvent -= RaiseLeftTriggerChangedEvent;
            mGamepadService.RaiseRightTriggerChangedEvent -= RaiseRightTriggerChangedEvent;
            mGamepadService.RaiseButtonChangedEvent -= RaiseButtonChangedEvent;
            mGamepadService.RaiseConnectedChangedEvent -= RaiseConnectedChangedEvent;

            mWebSocketClientsService.RaiseIsDisconnectionStatusChangedEvent -= RaiseIsDisconnectionStatusChangedEvent;
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
                UnsubscribeFromEvent();

                mLoggerService.Dispose();
                mWebSocketClientsService.Dispose();
                mGamepadService.Dispose();
            }
        }

        #endregion
    }
}
