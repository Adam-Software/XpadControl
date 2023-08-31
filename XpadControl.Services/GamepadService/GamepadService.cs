using System;
using System.Runtime.InteropServices;
using XpadControl.Services.GamepadService.EventArgs;
using XpadControl.Services.LoggerService;

namespace XpadControl.Services.GamepadService
{
    public class GamepadService :  IGamepadService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

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
                    mGamepadWindows.RunAsync();
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

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");

            mGamepadLinux?.Dispose();
            mGamepadWindows?.Dispose();
        }

        #region Linux gamepad event

        private void ButtonChanged(object sender, Gamepad.ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");

            OnRaiseButtonChangedEvent(e.Button, e.Pressed);
        }

        private void AxisChanged(object sender, Gamepad.AxisEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Axis} is {e.Value}");

            OnRaiseAxisChangedEvent(e.Axis, e.Value);
        }

        #endregion

        #region Windows gamepad event

        private void ButtonChanged(object sender, MaxRev.Input.Gamepad.ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");

            OnRaiseButtonChangedEvent((byte) e.Button, e.Pressed);
        }

        private void AxisChanged(object sender, MaxRev.Input.Gamepad.AxisEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Axis} is {e.Value}");

            OnRaiseAxisChangedEvent(Convert.ToByte(e.Axis), e.Value);
        }

        #endregion

        #region Raise events

        protected virtual void OnRaiseAxisChangedEvent(byte axis, short value)
        {
            AxisChangedEventHandler raiseEvent = RaiseAxisChangedEvent;

            AxisEventArgs eventArgs = new AxisEventArgs
            {
                 Axis = axis,
                 Value = value
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseButtonChangedEvent(byte button, bool pressed)
        {
            ButtonChangedEventHandler raiseEvent = RaiseButtonChangedEvent;

            ButtonEventArgs eventArgs = new ButtonEventArgs
            {
                 Button = button,
                 Pressed = pressed
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        #endregion

    }
}
