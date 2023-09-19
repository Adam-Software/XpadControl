using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using XpadControl.Common.Services.LoggerService;
using XpadControl.Common.Services.WebSocketService;
using XpadControl.Interfaces;
using System.Configuration;
using System.Reflection;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;
using XpadControl.Interfaces.Common.Dependencies.SettingsCollection;
using LinuxGamepadService = XpadControl.Linux.Services.GamepadService.GamepadService;
using WindowsGamepadService = XpadControl.Windows.Services.GamepadService.GamepadService;
using WindowsGamepadHostedService = XpadControl.Windows.Services.GamepadService.GamepadHostedService;
using LinuxGamepadHostedService = XpadControl.Linux.Services.GamepadService.GamepadHostedService;


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
                UpdateIntervalCollection intervalCollection = ReadUpdateIntervalFromSettings(configuration);

                builder.Services.AddSingleton<IWebSocketClientsService>(serviceProvider 
                    => new WebSocketClientsService(serviceProvider.GetService<ILoggerService>(), uri));

                // The background service reconnects when the connection is disconnected
                builder.Services.AddHostedService(serviceProvider 
                    => new WebSocketClientsHostedService(serviceProvider.GetService<IWebSocketClientsService>(), intervalCollection));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // The background service monitors the connection disconnection of the game controller
                    builder.Services.AddSingleton<IGamepadService>(serviceProvider 
                        => new LinuxGamepadService(serviceProvider.GetService<ILoggerService>()));
                    builder.Services.AddHostedService(serviceProvider => 
                        new LinuxGamepadHostedService(serviceProvider.GetService<IGamepadService>(), intervalCollection));
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // The Gamepad library for Windows requires background updates
                    builder.Services.AddSingleton<IGamepadService>(serviceProvider => 
                        new WindowsGamepadService(serviceProvider.GetService<ILoggerService>()));
                    builder.Services.AddHostedService(serviceProvider => 
                        new WindowsGamepadHostedService(serviceProvider.GetService<IGamepadService>(), intervalCollection));
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

        #region Read settings methods

        private static UriCollection ReadUriFromSettings(IConfigurationRoot configuration)
        {
            IConfigurationSection appSettings = configuration.GetRequiredSection("AppSettings");

            string webSocketHost = appSettings.GetValue<string>("WebSocketHost");
            int webSocketPort = appSettings.GetValue<int>("WebSocketPort");
            string wheelWebSocketPath = appSettings.GetValue<string>("WheelWebSocketPath");
            string servosWebSocketPath = appSettings.GetValue<string>("ServosWebSocketPath");

            UriCollection uriCollection = new(webSocketHost, webSocketPort, wheelWebSocketPath, servosWebSocketPath); 
            return uriCollection;
        }

        private static UpdateIntervalCollection ReadUpdateIntervalFromSettings(IConfigurationRoot configuration)
        {
            IConfigurationSection appSettings = configuration.GetRequiredSection("AppSettings");

            double windowGamepadUpdatePolling = appSettings.GetValue<double>("WindowGamepadUpdatePolling");
            int linuxGamepadUpdatePolling = appSettings.GetValue<int>("LinuxGamepadUpdatePolling");
            int websocketReconnectInterval = appSettings.GetValue<int>("WebsocketReconnectInterval");

            UpdateIntervalCollection updateIntervalCollection = new(linuxGamepadUpdatePolling, windowGamepadUpdatePolling, websocketReconnectInterval);
            return updateIntervalCollection;
        }

        #endregion

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