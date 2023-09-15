using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces.GamepadService;

namespace XpadControl.Linux.Services.GamepadService
{
    public class GamepadHostedService : BackgroundService
    {
        private readonly IGamepadService mGamepadService;

        public GamepadHostedService(IGamepadService mGamepadService)
        {
            this.mGamepadService = mGamepadService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    mGamepadService.Update();

                    // Gamepad library does not know how to
                    // connect/ disconnect to the gamepad on the fly, the logic of this is the ability to Update
                    await Task.Delay(2000);
                }

            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
