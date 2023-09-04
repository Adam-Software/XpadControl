using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using XpadControl.Common.Services.LoggerService;
using XpadControl.Common.Services.WebSocketCkientService;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.WebSocketCkientService;
using LinuxGamepadService = XpadControl.Linux.Services.GamepadService.GamepadService;
using WindowsGamepadService = XpadControl.Windows.Services.GamepadService.GamepadService;
using WindowsGamepadHostedService = XpadControl.Windows.Services.GamepadService.GamepadHostedService;

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

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    builder.Services.AddSingleton<IGamepadService>(new LinuxGamepadService(loogerService));
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //The Gamepad library for Windows requires background updates
                    builder.Services.AddSingleton<IGamepadService>(new WindowsGamepadService(loogerService));
                    builder.Services.AddHostedService<WindowsGamepadHostedService>();
                }

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