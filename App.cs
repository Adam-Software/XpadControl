using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.GamepadService.Dependencies.EventArgs;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.WebSocketCkientService;

namespace XpadControl
{
    public class App : IHostedService, IDisposable
    {
        private readonly IConfiguration mConfiguration;
        private readonly ILoggerService mLoggerService;
        private readonly IWebSocketClientsService mWebSocketClientsService;
        private readonly IGamepadService mGamepadService;
        private readonly IHostApplicationLifetime mApplicationLifetime;

        public App(IWebSocketClientsService webSocketClientsService, ILoggerService loggerService, IConfiguration configuration, 
            IGamepadService gamepadService, IHostApplicationLifetime applicationLifetime)
        {
            mConfiguration = configuration;
            mLoggerService = loggerService;
            mWebSocketClientsService = webSocketClientsService;
            mGamepadService = gamepadService;
            mApplicationLifetime = applicationLifetime;

            mApplicationLifetime.ApplicationStarted.Register(OnStarted);
            mApplicationLifetime.ApplicationStopped.Register(OnStopped);
            mApplicationLifetime.ApplicationStopping.Register(OnStopping);

            mGamepadService.RaiseButtonChangedEvent += RaiseButtonChangedEvent;
            mGamepadService.RaiseAxisChangedEvent += RaiseAxisChangedEvent;
        }

        private void RaiseAxisChangedEvent(object sender, AxisEventArgs e)
        {
            mLoggerService.WriteDebugLog($"RaiseAxisChangedEvent {e.Axis} is {e.Value}");
        }

        private void RaiseButtonChangedEvent(object sender, ButtonEventArgs e)
        {
            mLoggerService.WriteDebugLog($"RaiseButtonChangedEvent {e.Button} is {e.Pressed}");
        }

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

            
                /* example async process */

                /*Task.Run(async () =>
                {
                    try
                    {
                        // read AppSettings test
                        IConfigurationSection appSettings = mConfiguration.GetRequiredSection("AppSettings");
                        mLoggerService.WriteInformationLog("Setting version " + appSettings["Version"]);

                        // execute service
                        int testCalc = mWebSocketClientsService.CalculateCustomerAgs(1);
                        mLoggerService.WriteVerboseLog($"Websocket calculation is {testCalc}");

                        /*while (!cancellationToken.IsCancellationRequested)
                        {
                            mLoggerService.WriteVerboseLog("I do work");
                            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                        }*/

                /*}
                catch (TaskCanceledException)
                {
                    mLoggerService.WriteVerboseLog($"App canceled");
                }
                catch (Exception ex)
                {
                    mLoggerService.WriteErrorLog($"Unhandled exception when run app {ex}");
                }
            }, cancellationToken);*/

                return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            mLoggerService.WriteVerboseLog("Task complete and stop async called");
            return Task.CompletedTask;
        }

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
    }
}
