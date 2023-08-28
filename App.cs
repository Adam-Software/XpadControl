using Microsoft.Extensions.Configuration;
using XpadControl.Services;

namespace XpadControl
{
    public class App
    {
        private readonly IWebSocketClientsService WebSocketClientsService;
        private readonly IConfiguration Configuration;

        public App(IWebSocketClientsService webSocketClientsService, IConfiguration configuration)
        {
            WebSocketClientsService = webSocketClientsService;
            Configuration = configuration;
        }

        public void Run(string[] args)
        {
            // version settings
            var version = Configuration["Version"];
            Console.WriteLine("version " + version);

            var testCalc = WebSocketClientsService.CalculateCustomerAgs(1);
            Console.WriteLine(testCalc);
        }
    }
}
