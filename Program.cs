using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
            builder.Logging.ClearProviders();
            builder.Configuration.AddConfiguration(configuration);

            try
            {
                builder.Services.AddSingleton<ILoggerService>(loogerService);
                builder.Services.AddSingleton<IWebSocketClientsService>(new WebSocketClientsService(loogerService));
                builder.Services.AddSingleton<IGamepadService>(new GamepadService(loogerService));
                builder.Services.AddHostedService<App>();
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine("Service initialization error");
                Console.WriteLine(ex.ToString());
            }


            using IHost host = builder.Build();
            host.RunAsync().Wait();
            host.WaitForShutdownAsync();

        }
    }
}