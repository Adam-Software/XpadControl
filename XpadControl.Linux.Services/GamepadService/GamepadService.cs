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
        public event LeftAxisChangedEventHandler RaiseLeftAxisChangedEvent;
        public event RightAxisChangedEventHandler RaiseRightAxisChangedEvent;

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

            float x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            byte axys = e.Axis;

            switch (axys)
            {
                case 0:
                    x0 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"LEFT STICK X:{x0} or {e.Value}");
                    break;
                case 1:
                    y0 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"LEFT STICK Y:{y0} or {e.Value}");
                    break; 
                case 2:
                    x1 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"RIGHT STICK X:{x1} or {e.Value}");
                    break;
                case 3:
                    y1 = ConvertThumbToFloat(e.Value);
                    mLoggerService.WriteVerboseLog($"RIGHT STICK Y:{y1} or {e.Value}");
                    break;
            }

            switch (axys <=1)
            {
                case true:
                    OnRaiseLeftAxisChangedEvent(e.Axis, e.Value, x0, y0);
                    break;

                case false:
                    OnRaiseLeftAxisChangedEvent(e.Axis, e.Value, x1, y1);
                    break;
            }
        }

        #endregion

        #region Raise events

        protected virtual void OnRaiseRightAxisChangedEvent(byte axis, short value, float x, float y)
        {
            RightAxisChangedEventHandler raiseEvent = RaiseRightAxisChangedEvent;

            AxisEventArgs eventArgs = new()
            {
                Axis = axis,
                Value = value,
                X = x,
                Y = y
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        protected virtual void OnRaiseLeftAxisChangedEvent(byte axis, short value, float x, float y)
        {
            LeftAxisChangedEventHandler raiseEvent = RaiseLeftAxisChangedEvent;

            AxisEventArgs eventArgs = new()
            {
                 Axis = axis,
                 Value = value,
                 X = x,
                 Y = y
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
