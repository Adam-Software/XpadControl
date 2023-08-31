using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XpadControl.Services.GamepadService;
using XpadControl.Services.LoggerService;
using XpadControl.Services.WebSocketCkientService;

namespace XpadControl
{
    public class Program 
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var loogerService = new LoggerService(configuration);

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Configuration.AddConfiguration(configuration);
            builder.Services.AddSingleton<ILoggerService>(loogerService);
            builder.Services.AddSingleton<IWebSocketClientsService>(new WebSocketClientsService(loogerService));
            builder.Services.AddSingleton<IGamepadService>(new GamepadService(loogerService));
            builder.Services.AddHostedService<App>();

            using IHost host = builder.Build();

            host.RunAsync().Wait();

            /*Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    var loogerService = new LoggerService(configuration);

                    services.AddSingleton<ILoggerService>(loogerService);
                    services.AddSingleton<IWebSocketClientsService>(new WebSocketClientsService(loogerService));
                    services.AddSingleton<IGamepadService>(new GamepadService(loogerService));
                    services.AddHostedService<App>();
                })
                //.UseConsoleLifetime()
                //.ConfigureAppConfiguration(app => { app.AddConfiguration(configuration); })
                .Build().RunAsync();*/

        }
    }
}