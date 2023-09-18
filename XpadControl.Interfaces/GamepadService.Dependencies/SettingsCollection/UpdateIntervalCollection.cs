using System;

namespace XpadControl.Interfaces.GamepadService.Dependencies.SettingsCollection
{
    public class UpdateIntervalCollection
    {
        public UpdateIntervalCollection(int linuxGamepadUpdatePolling, double windowGamepadUpdatePolling) 
        {
            if (linuxGamepadUpdatePolling <= 0)
                linuxGamepadUpdatePolling = 2;

            if (windowGamepadUpdatePolling <= 0)
                windowGamepadUpdatePolling = 0.1;

            // *1000 - convert to ms
            LinuxGamepadUpdatePolling = linuxGamepadUpdatePolling * 1000;
            WindowGamepadUpdatePolling = (int)(Math.Round(windowGamepadUpdatePolling, 3) * 1000);
        }

        public int LinuxGamepadUpdatePolling { get; }
        public int WindowGamepadUpdatePolling { get; }
    }
}
