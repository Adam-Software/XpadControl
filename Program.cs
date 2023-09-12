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
using System.Configuration;
using System.Reflection;

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
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
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

            var loogerService = new LoggerService(configuration);

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Logging.ClearProviders();
            builder.Configuration.AddConfiguration(configuration);

            try
            {
                builder.Services.AddSingleton<ILoggerService>(loogerService);

                Uri uri = ReadUriFromSettings(configuration);
                builder.Services.AddSingleton<IWebSocketClientsService>(new WebSocketClientsService(loogerService, uri));

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

        private static Uri ReadUriFromSettings(IConfigurationRoot configuration)
        {
            IConfigurationSection appSettings = configuration.GetRequiredSection("AppSettings");

            string webSocketHost = appSettings["WebSocketHost"];
            string webSocketPort = appSettings["WebSocketPort"];
            string webSocketPath = appSettings["WebSocketPath"];

            if (string.IsNullOrEmpty(webSocketHost) || string.IsNullOrEmpty(webSocketPort) || string.IsNullOrEmpty(webSocketPath))
                throw new ConfigurationErrorsException("Error create uri from configuration");
            
            string ws = $"ws://{webSocketHost}:{webSocketPort}{webSocketPath}";

            Uri uri = new(ws);
            return uri;
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