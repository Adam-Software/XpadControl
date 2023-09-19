using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using XpadControl.Interfaces;
using XpadControl.Interfaces.Common.Dependencies.SettingsCollection;

namespace XpadControl.Linux.Services.GamepadService
{
    public class GamepadHostedService : BackgroundService
    {
        private readonly IGamepadService mGamepadService;
        private readonly UpdateIntervalCollection mUpdateIntervalCollection;

        public GamepadHostedService(IGamepadService gamepadService, UpdateIntervalCollection updateIntervalCollection)
        {
            mGamepadService = gamepadService;
            mUpdateIntervalCollection = updateIntervalCollection;
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
                    await Task.Delay(mUpdateIntervalCollection.LinuxGamepadUpdatePolling);
                }

            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
