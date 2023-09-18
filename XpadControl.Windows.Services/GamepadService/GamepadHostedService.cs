using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.GamepadService.Dependencies.SettingsCollection;

namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadHostedService : BackgroundService
    {
        private readonly IGamepadService mGamepadService;
        private readonly UpdateIntervalCollection mUpdateIntervalCollection;
        
        public GamepadHostedService(IGamepadService gamepadService, UpdateIntervalCollection updateIntervalCollection) 
        {
            mGamepadService = gamepadService;
            mUpdateIntervalCollection = updateIntervalCollection;

            Console.WriteLine($"Interval collection windows: {updateIntervalCollection.WindowGamepadUpdatePolling} linux {updateIntervalCollection.LinuxGamepadUpdatePolling}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    mGamepadService.Update();

                    // XInputium library requires updating every game frame
                    // 100 is the optimal parameter in the ratio of performance/response speed of the gamepad
                    await Task.Delay(mUpdateIntervalCollection.WindowGamepadUpdatePolling);
                }
                
            }, stoppingToken);
            
            return Task.CompletedTask;
        }
    }
}
