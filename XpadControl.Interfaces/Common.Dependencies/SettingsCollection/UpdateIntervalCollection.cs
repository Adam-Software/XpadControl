using System;

namespace XpadControl.Interfaces.Common.Dependencies.SettingsCollection
{
    public class UpdateIntervalCollection
    {
        private const int mMsConst = 1000;
        public UpdateIntervalCollection(int linuxGamepadUpdatePolling = 2, double windowGamepadUpdatePolling = 0.1, int websocketReconnectInterval = 5)
        {
            if (linuxGamepadUpdatePolling <= 0)
                linuxGamepadUpdatePolling = 2;

            if (windowGamepadUpdatePolling <= 0)
                windowGamepadUpdatePolling = 0.1;

            if(websocketReconnectInterval <= 0)
                websocketReconnectInterval = 5;

            // *1000 - convert to ms
            LinuxGamepadUpdatePolling = linuxGamepadUpdatePolling * mMsConst;
            WindowGamepadUpdatePolling = (int)(Math.Round(windowGamepadUpdatePolling, 3) * mMsConst);
            WebsocketReconnectInterval = websocketReconnectInterval * mMsConst;
        }

        public int LinuxGamepadUpdatePolling { get; }
        public int WindowGamepadUpdatePolling { get; }
        public int WebsocketReconnectInterval { get; }
    }
}
