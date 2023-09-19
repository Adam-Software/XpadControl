using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.Common.Dependencies.SettingsCollection;
using XpadControl.Interfaces;


namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadHostedService : BackgroundService
    {
        private readonly IGamepadService mGamepadService;
        private readonly int mWindowGamepadUpdatePolling;
        
        public GamepadHostedService(IGamepadService gamepadService, UpdateIntervalCollection updateIntervalCollection) 
        {
            mGamepadService = gamepadService;
            mWindowGamepadUpdatePolling = updateIntervalCollection.WindowGamepadUpdatePolling;
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
                    await Task.Delay(mWindowGamepadUpdatePolling);
                }
                
            }, stoppingToken);
            
            return Task.CompletedTask;
        }
    }
}
