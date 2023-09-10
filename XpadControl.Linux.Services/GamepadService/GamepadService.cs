using Gamepad;
using System;
using XpadControl.Interfaces.GamepadService;
using XpadControl.Interfaces.LoggerService;
using XpadControl.Interfaces.GamepadService.Dependencies.Extensions;
using MyAxisEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.AxisEventArgs;
using MyButtonEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ButtonEventArgs;

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

        public void Dispose()
        {
            mLoggerService.WriteVerboseLog($"Dispose {nameof(GamepadService)} called");

            mGamepad?.Dispose();
        }


        #region Gamepad event

        private void ButtonChanged(object sender, ButtonEventArgs e)
        {
            mLoggerService.WriteVerboseLog($"{e.Button} is {e.Pressed}");

            OnRaiseButtonChangedEvent(e.Button, e.Pressed);
        }

        private void AxisChanged(object sender, AxisEventArgs e)
        {
            float x0 = 0, y0 = 0, x1 = 0, y1 = 0;
            byte axis = e.Axis;
            short value = e.Value;

            mLoggerService.WriteVerboseLog($"{axis} is {value}");

            switch (axis)
            {
                case 0:
                    x0 = value.ToFloat();
                    mLoggerService.WriteVerboseLog($"LEFT STICK X:{x0} or {value}");
                    break;
                case 1:
                    y0 = -value.ToFloat();
                    mLoggerService.WriteVerboseLog($"LEFT STICK Y:{y0} or {value}");
                    break; 
                case 2:
                    x1 = value.ToFloat();
                    mLoggerService.WriteVerboseLog($"RIGHT STICK X:{x1} or {value}");
                    break;
                case 3:
                    y1 = -value.ToFloat();
                    mLoggerService.WriteVerboseLog($"RIGHT STICK Y:{y1} or {value}");
                    break;
            }

            switch (axis <=1)
            {
                case true:
                    OnRaiseLeftAxisChangedEvent(axis, value, x0, y0);
                    break;

                case false:
                    OnRaiseRightAxisChangedEvent(axis, value, x1, y1);
                    break;
            }
        }

        #endregion

        #region Raise events

        protected virtual void OnRaiseRightAxisChangedEvent(byte axis, short value, float x, float y)
        {
            RightAxisChangedEventHandler raiseEvent = RaiseRightAxisChangedEvent;

            MyAxisEventArgs eventArgs = new()
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

            MyAxisEventArgs eventArgs = new()
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

            MyButtonEventArgs eventArgs = new()
            {
                 Button = button,
                 Pressed = pressed
            };

            raiseEvent?.Invoke(this, eventArgs);
        }

        #endregion

    }
}
