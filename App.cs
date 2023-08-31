using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using XpadControl.Services.GamepadService;
using XpadControl.Services.LoggerService;
using XpadControl.Services.WebSocketCkientService;

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

            // register to lifetime callbacks
            mApplicationLifetime.ApplicationStarted.Register(OnStarted);
            mApplicationLifetime.ApplicationStopped.Register(OnStopped);
            mApplicationLifetime.ApplicationStopping.Register(OnStopping);
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
            /*simple realization*/
            /*try
            {
                // read AppSettings test
                IConfigurationSection appSettings = mConfiguration.GetRequiredSection("AppSettings");
                mLoggerService.WriteInformationLog("Setting version " + appSettings["Version"]);

                // execute service
                int testCalc = mWebSocketClientsService.CalculateCustomerAgs(1);
                mLoggerService.WriteVerboseLog($"Websocket calculation is {testCalc}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    mLoggerService.WriteVerboseLog("I do work");
                    Task.Delay(TimeSpan.FromSeconds(10)).Wait();
                }
            }
            catch (Exception ex) 
            {
                mLoggerService.WriteErrorLog($"Error when run app {ex.Message}");
            }
            finally 
            {
                mApplicationLifetime.StopApplication();
            }*/

            Task.Run(async () =>
            {
                try
                {
                    // read AppSettings test
                    IConfigurationSection appSettings = mConfiguration.GetRequiredSection("AppSettings");
                    mLoggerService.WriteInformationLog("Setting version " + appSettings["Version"]);

                    // execute service
                    int testCalc = mWebSocketClientsService.CalculateCustomerAgs(1);
                    mLoggerService.WriteVerboseLog($"Websocket calculation is {testCalc}");

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        mLoggerService.WriteVerboseLog("I do work");
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }

                }
                catch (TaskCanceledException)
                {
                    mLoggerService.WriteVerboseLog($"App canceled");
                }
                catch (Exception ex)
                {
                    mLoggerService.WriteErrorLog($"Unhandled exception when run app {ex}");
                }
            }, cancellationToken);

           
            return Task.CompletedTask;
        }

        /// <summary>
        /// Dispose all services
        /// </summary>
        public void Dispose()
        {
            mLoggerService.WriteVerboseLog("Coomon dispose called");

            mLoggerService.Dispose();
            mWebSocketClientsService.Dispose();
            mGamepadService.Dispose();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            mLoggerService.WriteVerboseLog("Task complete and stop async called");
            return Task.CompletedTask;
        }
    }
}
