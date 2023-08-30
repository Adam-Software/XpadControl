using Microsoft.Extensions.Configuration;
using XpadControl.Services.LoggerService;
using XpadControl.Services.WebSocketCkientService;

namespace XpadControl
{
    public class App
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

        public void Run(string[] args)
        {
            // version settings
            IConfigurationSection appSettings = Configuration.GetRequiredSection("AppSettings");
            LoggerService.WriteInformationLog("version " + appSettings["Version"]);

            var testCalc = WebSocketClientsService.CalculateCustomerAgs(1);
            LoggerService.WriteVerboseLog($"{testCalc}");
        }
    }
}
