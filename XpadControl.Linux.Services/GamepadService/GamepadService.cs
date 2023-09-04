using Gamepad;
using System;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.LoggerService;
using AxisEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.AxisEventArgs;
using ButtonEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ButtonEventArgs;

namespace XpadControl.Linux.Services.GamepadService
{
    public class GamepadService : IGamepadService
    {
        public event AxisChangedEventHandler RaiseAxisChangedEvent;
        public event ButtonChangedEventHandler RaiseButtonChangedEvent;

        private readonly GamepadController mGamepad;
        private readonly ILoggerService mLoggerService;

        public GamepadService(ILoggerService loggerService) 
        {
            mLoggerService = loggerService;

            mLoggerService.WriteVerboseLog("OS is Linux");
           
            try
            {
                mGamepad = new GamepadController("/dev/input/js0");    
            }
            catch (Exception ex)
            {
                mLoggerService.WriteErrorLog($"Error when bind gamepad {ex.Message}");
            }
            

            if (mGamepad != null)
            {
                mGamepad.AxisChanged += AxisChanged;
                mGamepad.ButtonChanged += ButtonChanged;
            }    
        }

        private static float ConvertThumbToFloat(short axis)
        {
            return ((float)axis) / (axis >= 0 ? 32767 : 32768);
        }

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");

            mGamepad?.Dispose();
        }


        #region Gamepad event

        private void ButtonChanged(object sender, Gamepad.ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");

            OnRaiseButtonChangedEvent(e.Button, e.Pressed);
        }

        private void AxisChanged(object sender, Gamepad.AxisEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Axis} is {e.Value}");

            switch (e.Axis)
            {
                case 0:
                    float x0 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"LEFT STICK X:{x0} or {e.Value}");
                    break;
                case 1:
                    float y0 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"LEFT STICK Y:{y0} or {e.Value}");
                    break; 
                case 2:
                    float x1 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"RIGHT STICK X:{x1} or {e.Value}");
                    break;
                case 3:
                    float y1 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"RIGHT STICK Y:{y1} or {e.Value}");
                    break;
            }

            OnRaiseAxisChangedEvent(e.Axis, e.Value);
        }

        #endregion

        #region Raise events

        protected virtual void OnRaiseAxisChangedEvent(byte axis, short value)
        {
            AxisChangedEventHandler raiseEvent = RaiseAxisChangedEvent;

            AxisEventArgs eventArgs = new()
            {
                 Axis = axis,
                 Value = value
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseButtonChangedEvent(byte button, bool pressed)
        {
            ButtonChangedEventHandler raiseEvent = RaiseButtonChangedEvent;

            ButtonEventArgs eventArgs = new()
            {
                 Button = button,
                 Pressed = pressed
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        #endregion

    }
}
