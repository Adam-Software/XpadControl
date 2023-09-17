using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using XpadControl.Common.Services.LoggerService;
using XpadControl.Common.Services.WebSocketService;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.WebSocketCkientService;
using System.Configuration;
using System.Reflection;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;
using XpadControl.Interfaces.GamepadService.Dependencies.SettingsCollection;

using LinuxGamepadService = XpadControl.Linux.Services.GamepadService.GamepadService;
using WindowsGamepadService = XpadControl.Windows.Services.GamepadService.GamepadService;
using WindowsGamepadHostedService = XpadControl.Windows.Services.GamepadService.GamepadHostedService;
using LinuxGamepadHostedService = XpadControl.Linux.Services.GamepadService.GamepadHostedService;
using System.Net.Http.Headers;

namespace XpadControl
{
    public class Program 
    {
        private static ProgramArguments mProgramArguments;

        static void Main(string[] args)
        {
            try
            {
                if (!ParseArguments(args))
                    return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(mProgramArguments.ConfigPathName)
                .Build();

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Logging.ClearProviders();
            builder.Configuration.AddConfiguration(configuration);

            try
            {
                builder.Services.AddSingleton<ILoggerService>(new LoggerService(configuration));

                UriCollection uri = ReadUriFromSettings(configuration);
                builder.Services.AddSingleton<IWebSocketClientsService>(serviceProvider 
                    => new WebSocketClientsService(serviceProvider.GetService<ILoggerService>(), uri));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // the background service monitors the connection disconnection of the game controller
                    builder.Services.AddSingleton<IGamepadService>(serviceProvider 
                        => new LinuxGamepadService(serviceProvider.GetService<ILoggerService>()));
                    builder.Services.AddHostedService(serviceProvider => 
                        new LinuxGamepadHostedService(serviceProvider.GetService<IGamepadService>(), new UpdateIntervalCollection(10, 10)));
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //The Gamepad library for Windows requires background updates
                    builder.Services.AddSingleton<IGamepadService>(serviceProvider => 
                        new WindowsGamepadService(serviceProvider.GetService<ILoggerService>()));
                    builder.Services.AddHostedService(serviceProvider => 
                        new WindowsGamepadHostedService(serviceProvider.GetService<IGamepadService>(), new UpdateIntervalCollection(10, 10)));
                }

                builder.Services.AddHostedService<App>();
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.Error.WriteLine("Configuraton read error");
                Console.Error.WriteLine(ex.ToString());
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine("Service initialization error");
                Console.Error.WriteLine(ex.ToString());
            }

            using IHost host = builder.Build();
            host.RunAsync().Wait();
            host.WaitForShutdownAsync();
        }

        private static UriCollection ReadUriFromSettings(IConfigurationRoot configuration)
        {
            IConfigurationSection appSettings = configuration.GetRequiredSection("AppSettings");

            string webSocketHost = appSettings["WebSocketHost"];
            string webSocketPort = appSettings["WebSocketPort"];
            string wheelWebSocketPath = appSettings["WheelWebSocketPath"];
            string servosWebSocketPath = appSettings["ServosWebSocketPath"];

            UriCollection uriCollection = new(webSocketHost, webSocketPort, wheelWebSocketPath, servosWebSocketPath); 
            return uriCollection;
        }

        private static bool ParseArguments(string[] args)
        {
            mProgramArguments = new();
            CommandLineParser.CommandLineParser parser = new()
            {
                IgnoreCase = true,
            };

            parser.ExtractArgumentAttributes(mProgramArguments);
            parser.ParseCommandLine(args);

            if (mProgramArguments.ShowVersion)
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                Console.WriteLine($"{v}");
                return false;
            }

            if (mProgramArguments.ShowHelp)
            {
                parser.ShowUsage();
                return false;
            }

            if (!Path.Exists(mProgramArguments.ConfigPathName))
                throw new FileNotFoundException($"Cannot find app settings file {mProgramArguments.ConfigPathName}");

            return true;

        }
    }
}