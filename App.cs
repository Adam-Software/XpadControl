using Microsoft.Extensions.Configuration;
using XpadControl.Services.LoggerService;
using XpadControl.Services.WebSocketCkientService;

namespace XpadControl
{
    public class App : IDisposable
    {
        private readonly IWebSocketClientsService WebSocketClientsService;
        private readonly ILoggerService LoggerService;
        private readonly IConfiguration Configuration;

        public App(IWebSocketClientsService webSocketClientsService, ILoggerService loggerService, IConfiguration configuration)
        {
            WebSocketClientsService = webSocketClientsService;
            LoggerService = loggerService;
            Configuration = configuration;
        }

        public Task RunAsync(string[] args)
        {
            // read AppSettings test
            IConfigurationSection appSettings = Configuration.GetRequiredSection("AppSettings");
            LoggerService.WriteInformationLog("version " + appSettings["Version"]);

            // execute service
            var testCalc = WebSocketClientsService.CalculateCustomerAgs(1);
            LoggerService.WriteVerboseLog($"{testCalc}");

            // imitation work
            Task.Delay(10000).Wait();

            LoggerService.WriteInformationLog("Task complete");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            LoggerService.WriteVerboseLog("Dispose called");
            LoggerService.Dispose();
        }
    }
}
