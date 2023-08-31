using Microsoft.Extensions.Configuration;
using XpadControl.Services.GamepadService;
using XpadControl.Services.LoggerService;
using XpadControl.Services.WebSocketCkientService;

namespace XpadControl
{
    public class App : IDisposable
    {
        private readonly IConfiguration mConfiguration;
        private readonly ILoggerService mLoggerService;
        private readonly IWebSocketClientsService mWebSocketClientsService;
        private readonly IGamepadService mGamepadService;
        
        public App(IWebSocketClientsService webSocketClientsService, ILoggerService loggerService, IConfiguration configuration, IGamepadService gamepadService)
        {
            mConfiguration = configuration;
            mLoggerService = loggerService;
            mWebSocketClientsService = webSocketClientsService;
            mGamepadService = gamepadService;
        }

        public Task RunAsync(string[] args)
        {
            // read AppSettings test
            IConfigurationSection appSettings = mConfiguration.GetRequiredSection("AppSettings");
            mLoggerService.WriteInformationLog("Setting version " + appSettings["Version"]);

            // execute service
            int testCalc = mWebSocketClientsService.CalculateCustomerAgs(1);
            mLoggerService.WriteVerboseLog($"Websocket calculation is {testCalc}");

            // imitation work
            Task.Delay(10000).Wait();

            mLoggerService.WriteInformationLog("Task complete");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Dispose all services
        /// </summary>
        public void Dispose()
        {
            mLoggerService.WriteVerboseLog("Dispose called");

            mLoggerService.Dispose();
            mWebSocketClientsService.Dispose();
            mGamepadService.Dispose();
        }
    }
}
