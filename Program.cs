﻿using Microsoft.Extensions.Configuration;
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
            IHost host = CreateHostBuilder(args).Build();
            IServiceScope scope = host.Services.CreateScope();

            IServiceProvider services = scope.ServiceProvider;
            ILoggerService logger = services.GetRequiredService<ILoggerService>();
            App app = services.GetRequiredService<App>();

            try
            {
                logger.WriteInformationLog("App normally start");
                app.RunAsync(args).Wait();   
                
            }
            catch (Exception e)
            {
                logger.WriteErrorLog($"Exception on app start {e.Message}");
                app.Dispose();
            }
            finally 
            {
                logger.WriteInformationLog("App normally stop");
                app.Dispose();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] strings)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<ILoggerService>(new LoggerService(configuration));
                    services.AddSingleton<IWebSocketClientsService, WebSocketClientsService>();
                    services.AddSingleton<IGamepadService, GamepadService>();
                    services.AddSingleton<App>();
                })
                .ConfigureAppConfiguration(app => { app.AddConfiguration(configuration); } );
        }
    }
}