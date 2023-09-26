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
using XpadControl.Common.Services.BindingButtonsService;

namespace XpadControl
{
    public class Program 
    {
        private static readonly ProgramArguments mProgramArguments = new();

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
                .AddJsonFile(mProgramArguments.ConfigPath)
                .Build();

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Logging.ClearProviders();
            builder.Configuration.AddConfiguration(configuration);
            
            try
            {
                builder.Services.AddSingleton<ILoggerService>(new LoggerService(configuration));

                var appSettingsSection = configuration.GetRequiredSection("AppSettings");

                UriCollection uri = ReadUriFromSettings(appSettingsSection);
                UpdateIntervalCollection intervalCollection = ReadUpdateIntervalFromSettings(appSettingsSection);
                
                builder.Services.AddSingleton<IWebSocketClientsService>(serviceProvider 
                    => new WebSocketClientsService(serviceProvider.GetService<ILoggerService>(), uri));

                // The background service reconnects when the connection is disconnected
                builder.Services.AddHostedService(serviceProvider => 
                        new WebSocketClientsHostedService(serviceProvider.GetService<IWebSocketClientsService>(), intervalCollection));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // The background service monitors the connection disconnection of the game controller
                    builder.Services.AddSingleton<IGamepadService>(serviceProvider => 
                        new LinuxGamepadService(serviceProvider.GetService<ILoggerService>()));
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

                PathCollection pathCollection = ReadPathCollectionFromSettings(appSettingsSection);

                builder.Services.AddSingleton<IBindingButtonsService>(serviceProvider =>
                    new BindingButtonsService(serviceProvider.GetService<ILoggerService>(), pathCollection.ButtonBindingsConfigPath));

                builder.Services.AddHostedService(serviceProvider => 
                        new App(serviceProvider.GetService<IWebSocketClientsService>(), 
                                serviceProvider.GetService<ILoggerService>(), 
                                serviceProvider.GetService<IGamepadService>(),
                                serviceProvider.GetService<IBindingButtonsService>(),
                                serviceProvider.GetService<IHostApplicationLifetime>(), 
                                pathCollection));
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

        private static UriCollection ReadUriFromSettings(IConfigurationSection appSettings)
        {
            IConfigurationSection socketClientOptions = appSettings.GetRequiredSection("WebSocketClientOptions");

            string webSocketHost = socketClientOptions.GetValue<string>("WebSocketHost");
            int webSocketPort = socketClientOptions.GetValue<int>("WebSocketPort");
            string wheelWebSocketPath = socketClientOptions.GetValue<string>("WheelWebSocketPath");
            string servosWebSocketPath = socketClientOptions.GetValue<string>("ServosWebSocketPath");

            UriCollection uriCollection = new(webSocketHost, webSocketPort, wheelWebSocketPath, servosWebSocketPath); 
            return uriCollection;
        }

        private static UpdateIntervalCollection ReadUpdateIntervalFromSettings(IConfigurationSection appSettings)
        {
            IConfigurationSection pollingOptions = appSettings.GetSection("GamepadPollingOptions");
            IConfigurationSection socketClientOptions = appSettings.GetRequiredSection("WebSocketClientOptions");

            double windowGamepadUpdatePolling = pollingOptions.GetValue<double>("WindowGamepadUpdatePolling");
            int linuxGamepadUpdatePolling = pollingOptions.GetValue<int>("LinuxGamepadUpdatePolling");
            int websocketReconnectInterval = socketClientOptions.GetValue<int>("WebsocketReconnectInterval");

            UpdateIntervalCollection updateIntervalCollection = new(linuxGamepadUpdatePolling, windowGamepadUpdatePolling, websocketReconnectInterval);
            return updateIntervalCollection;
        }

        private static PathCollection ReadPathCollectionFromSettings(IConfigurationSection appSettings)
        {
            IConfigurationSection optionsSection = appSettings.GetSection("PathOptions");
            string zeroConfigPath = optionsSection.GetValue<string>("AdamZeroPositionConfig");
            string buttonBindingConfigPath = optionsSection.GetValue<string>("ButtonBindingsConfig");

            PathCollection appArguments = new(zeroConfigPath, buttonBindingConfigPath);
            
            return appArguments;
        }

        #endregion

        #region Read program arguments

        private static bool ParseArguments(string[] args)
        {
            CommandLineParser.CommandLineParser parser = new()
            {
                IgnoreCase = true,
            };

            parser.ExtractArgumentAttributes(mProgramArguments);
            parser.ParseCommandLine(args);

            if (!Path.Exists(mProgramArguments.ConfigPath))
                throw new FileNotFoundException($"Cannot find app settings file {mProgramArguments.ConfigPath}");

            if (mProgramArguments.ShowConfigPath)
            {
                Console.WriteLine($"Setting loaded from {mProgramArguments.ConfigPath}");
                return false; 
            }

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

            return true;
        }

        #endregion
    }

}