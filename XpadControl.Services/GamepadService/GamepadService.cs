using System;
using System.Runtime.InteropServices;
using XpadControl.Services.LoggerService;

namespace XpadControl.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        private readonly MaxRev.Input.Gamepad.GamepadController? mGamepadWindows;
        private readonly Gamepad.GamepadController? mGamepadLinux;
        private readonly ILoggerService mLoggerService;

        public GamepadService(ILoggerService loggerService) 
        {
            mLoggerService = loggerService;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                mLoggerService.WriteVerboseLog("OS is Linux");
                
                try
                {
                    mGamepadLinux = new Gamepad.GamepadController("/dev/input/js0");
                }
                catch (Exception ex)
                {
                    mLoggerService.WriteErrorLog($"Error when bind gamepad {ex.Message}");
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                mLoggerService.WriteVerboseLog("OS is Windows");

                try
                {
                    mGamepadWindows = new MaxRev.Input.Gamepad.GamepadController();
                }
                catch (Exception ex)
                {
                    mLoggerService.WriteErrorLog($"Error when bind gamepad {ex.Message}");
                }
            }

            if (mGamepadLinux != null)
            {
                mGamepadLinux.AxisChanged += AxisChanged;
                mGamepadLinux.ButtonChanged += ButtonChanged;
            }    

            if(mGamepadWindows != null)
            {
                mGamepadWindows.AxisChanged += AxisChanged;
                mGamepadWindows.ButtonChanged += ButtonChanged; 
            }
            
        }

        #region Linux gamepad event

        private void ButtonChanged(object sender, Gamepad.ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");
        }

        private void AxisChanged(object sender, Gamepad.AxisEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Axis} is {e.Value}");
        }

        #endregion

        #region Windows gamepad event

        private void ButtonChanged(object sender, MaxRev.Input.Gamepad.ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");
        }

        private void AxisChanged(object sender, MaxRev.Input.Gamepad.AxisEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Axis} is {e.Value}");
        }

        #endregion

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");

            mGamepadLinux?.Dispose();
            mGamepadWindows?.Dispose();
        }
    }
}
