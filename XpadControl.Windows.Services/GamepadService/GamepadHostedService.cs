using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.GamepadService;

namespace XpadControl.Windows.Services.GamepadService
{
    public class GamepadHostedService : BackgroundService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

        private readonly IGamepadService mGamepadService;
        public GamepadHostedService(IGamepadService gamepadService) 
        {
            mGamepadService = gamepadService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    mGamepadService.Update();

                    // XInputium library requires updating every game frame
                    // 100 is the optimal parameter in the ratio of performance / response speed of the gamepad
                    await Task.Delay(100);
                }
                
            }, stoppingToken);
            
            return Task.CompletedTask;
        }
    }
}
